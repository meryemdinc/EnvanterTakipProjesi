using Application.Interfaces.Services;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs.Maintenances;
using Domain.Entities;
using Domain.Enums;
using Application.Exceptions;

namespace Application.Services
{
    public class MaintenanceService(IMapper mapper, IUnitOfWork unitOfWork) : IMaintenanceService
    {
        public async Task<List<MaintenanceDto>> GetAllMaintenancesAsync()
        {
            var maintenances = await unitOfWork.Maintenances.GetAllAsync();
            return mapper.Map<List<MaintenanceDto>>(maintenances);
        }

        public async Task<MaintenanceDto> GetByIdAsync(int id)
        {
            var maintenance = await unitOfWork.Maintenances.GetByIdAsync(id);
            if (maintenance == null)
            {
                throw new NotFoundException("Aranan bakım kaydı bulunamadı.");
            }
            return mapper.Map<MaintenanceDto>(maintenance);
        }

        public async Task<List<MaintenanceDto>> GetHistoryByItemIdAsync(int inventoryItemId)
        {
            var maintenances = await unitOfWork.Maintenances.FindAsync(m => m.InventoryItemId == inventoryItemId);
            return mapper.Map<List<MaintenanceDto>>(maintenances);
        }

        public async Task<List<MaintenanceDto>> GetActiveMaintenancesAsync()
        {
            var activeMaintenances = await unitOfWork.Maintenances.FindAsync(m => m.RepairedAt == null);
            return mapper.Map<List<MaintenanceDto>>(activeMaintenances);
        }

        public async Task CreateAsync(CreateMaintenanceDto createMaintenanceDto)
        {
            // 1. Önce cihazı bulalım
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(createMaintenanceDto.InventoryItemId);
            if (inventoryItem == null)
            {
                throw new NotFoundException("Bakıma alınmak istenen eşya bulunamadı.");
            }

            // 2. Cihaz şu an zimmetli mi? (Zimmetli cihaz önce iade edilmeli)
            if (inventoryItem.Status == ItemStatus.Assigned)
            {
                throw new BadRequestException("Zimmetli olan bir cihaz doğrudan bakıma alınamaz. Önce zimmet iadesi yapmalısınız.");
            }

            // 3. Cihaz zaten bakımda mı?
            var existingActiveMaintenance = await unitOfWork.Maintenances.FindAsync(m =>
                m.InventoryItemId == createMaintenanceDto.InventoryItemId && m.RepairedAt == null);

            if (existingActiveMaintenance.Any())
            {
                throw new BadRequestException("Bu cihaza ait devam eden aktif bir bakım kaydı zaten mevcut.");
            }

            // 4. Her şey yolunda, cihazı bakıma al!
            inventoryItem.Status = ItemStatus.Maintenance;
            unitOfWork.InventoryItems.Update(inventoryItem);

            var maintenance = mapper.Map<Maintenance>(createMaintenanceDto);
            await unitOfWork.Maintenances.AddAsync(maintenance);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMaintenanceDto updateMaintenanceDto)
        {
            var existingMaintenance = await unitOfWork.Maintenances.GetByIdAsync(updateMaintenanceDto.Id);
            if (existingMaintenance == null)
            {
                throw new NotFoundException("Güncellenecek bakım kaydı bulunamadı.");
            }

            // DÜZELTME 3: Erken Taburcu Etme! Sadece yeni veride RepairedAt DOLU geldiğinde durumu değiştir.
            // Ayrıca eski kaydın RepairedAt değerinin boş olduğundan emin ol ki, zaten tamir edilmiş cihazı tekrar Available yapmaya çalışmasın.
            if (existingMaintenance.RepairedAt == null && updateMaintenanceDto.RepairedAt != null)
            {
                var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(existingMaintenance.InventoryItemId);
                if (inventoryItem != null)
                {
                    inventoryItem.Status = ItemStatus.Available;
                    unitOfWork.InventoryItems.Update(inventoryItem);
                }
            }

            mapper.Map(updateMaintenanceDto, existingMaintenance);
            unitOfWork.Maintenances.Update(existingMaintenance);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingMaintenance = await unitOfWork.Maintenances.GetByIdAsync(id);
            if (existingMaintenance == null)
            {
                throw new NotFoundException("Silinecek bakım kaydı bulunamadı.");
            }

            // redundant kontrol (existingMaintenance != null) silindi
            if (existingMaintenance.RepairedAt != null)
            {
                throw new BadRequestException("Bu kaydı silemezsin, cihazın bakımı tamamlanmış.");
            }

            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(existingMaintenance.InventoryItemId);
            if (inventoryItem != null && inventoryItem.Status == ItemStatus.Maintenance)
            {
                // Bakım iptal edildiğine göre cihazı "Hasarlı" durumuna geri çekiyoruz. 
                inventoryItem.Status = ItemStatus.Damaged;
                unitOfWork.InventoryItems.Update(inventoryItem);
            }

            existingMaintenance.IsDeleted = true; // Soft delete
            unitOfWork.Maintenances.Update(existingMaintenance);
            await unitOfWork.SaveChangesAsync();
        }
    }
}