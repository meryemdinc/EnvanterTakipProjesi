using Application.Interfaces.Services;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs.Maintenances;
using Domain.Entities;
using Domain.Enums;

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
                throw new Exception("Aranan bakım kaydı bulunamadı.");
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
            // DÜZELTME 1 & 2: Eşyanın aktif (RepairedAt null) bir bakım kaydı var mı diye FindAsync ile bakıyoruz!
            var existingActiveMaintenance = await unitOfWork.Maintenances.FindAsync(m =>
                m.InventoryItemId == createMaintenanceDto.InventoryItemId && m.RepairedAt == null);

            // Any() kullanarak null hatası almaktan kurtuluyoruz.
            if (existingActiveMaintenance.Any())
            {
                throw new Exception("Bu cihaza ait devam eden aktif bir bakım kaydı zaten mevcut.");
            }

            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(createMaintenanceDto.InventoryItemId);
            if (inventoryItem == null)
            {
                throw new Exception("Bakıma alınmak istenen eşya bulunamadı.");
            }

            // Eşyayı bakıma alıyoruz!
            inventoryItem.Status = ItemStatus.Maintenance;
            unitOfWork.InventoryItems.Update(inventoryItem); // Eşya durumunu güncellemeyi unutma!

            var maintenance = mapper.Map<Maintenance>(createMaintenanceDto);
            await unitOfWork.Maintenances.AddAsync(maintenance);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMaintenanceDto updateMaintenanceDto)
        {
            var existingMaintenance = await unitOfWork.Maintenances.GetByIdAsync(updateMaintenanceDto.Id);
            if (existingMaintenance == null)
            {
                throw new Exception("Güncellenecek bakım kaydı bulunamadı.");
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
                throw new Exception("Silinecek bakım kaydı bulunamadı.");
            }

            // redundant kontrol (existingMaintenance != null) silindi
            if (existingMaintenance.RepairedAt != null)
            {
                throw new Exception("Bu kaydı silemezsin, cihazın bakımı tamamlanmış.");
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