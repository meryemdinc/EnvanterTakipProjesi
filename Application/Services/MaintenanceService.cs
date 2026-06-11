using Application.DTOs.Maintenances;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers;
using AutoMapper;
using Domain.Entities;
using MassTransit;
using Domain.Enums;
using Application.Messages;

namespace Application.Services
{
    // DİKKAT: IPublishEndpoint eklendi!
    public class MaintenanceService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        MaintenanceManager maintenanceManager,
        IPublishEndpoint publishEndpoint) : IMaintenanceService
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

            // 2. KURALLARI ÇALIŞTIR
            maintenanceManager.CheckIfItemIsEligibleForMaintenance(inventoryItem);
            await maintenanceManager.CheckIfItemAlreadyInMaintenanceAsync(createMaintenanceDto.InventoryItemId);

            // 3. İŞ AKIŞI: Cihazı bakıma al!
            inventoryItem.Status = ItemStatus.Maintenance;
            unitOfWork.InventoryItems.Update(inventoryItem);

            var maintenance = mapper.Map<Maintenance>(createMaintenanceDto);
            await unitOfWork.Maintenances.AddAsync(maintenance);
            await unitOfWork.SaveChangesAsync();

            // 4. 🚀 RABBITMQ'YA MESAJ FIRLAT (Değişkenler düzeltildi)
            await publishEndpoint.Publish(new MaintenanceStartedEvent
            {
                ItemId = inventoryItem.Id,
                ItemName = $"{inventoryItem.Brand} {inventoryItem.Model}".Trim(),
                EmployeeEmail = "it-destek@sirket.com", // Şimdilik statik IT e-postası (Test için)

                // NOT: Eğer DTO sınıfında 'Description' yerine 'Notes' yazıyorsa burayı createMaintenanceDto.Notes olarak değiştir!
                MaintenanceReason = createMaintenanceDto.Description
            });
        }

        public async Task UpdateAsync(UpdateMaintenanceDto updateMaintenanceDto)
        {
            var existingMaintenance = await unitOfWork.Maintenances.GetByIdAsync(updateMaintenanceDto.Id);
            if (existingMaintenance == null)
            {
                throw new NotFoundException("Güncellenecek bakım kaydı bulunamadı.");
            }

            // İŞ AKIŞI: Erken Taburcu Etme
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

            // 1. KURALI ÇALIŞTIR
            maintenanceManager.CheckIfMaintenanceCanBeDeleted(existingMaintenance);

            // 2. İŞ AKIŞI: Bakım iptal edildiğine göre cihazı "Hasarlı" durumuna geri çek
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(existingMaintenance.InventoryItemId);
            if (inventoryItem != null && inventoryItem.Status == ItemStatus.Maintenance)
            {
                inventoryItem.Status = ItemStatus.Damaged;
                unitOfWork.InventoryItems.Update(inventoryItem);
            }

            // 3. SİL (Soft Delete)
            existingMaintenance.IsDeleted = true;
            unitOfWork.Maintenances.Update(existingMaintenance);
            await unitOfWork.SaveChangesAsync();
        }
    }
}