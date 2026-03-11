using Application.DTOs.Maintenances;

namespace Application.Interfaces.Services
{
    public interface IMaintenanceService
    {
        Task<List<MaintenanceDto>> GetAllMaintenancesAsync();
        Task<MaintenanceDto> GetByIdAsync(int id);

        // Bir cihaza ait tüm geçmiş bakımları getir
        Task<List<MaintenanceDto>> GetHistoryByItemIdAsync(int inventoryItemId);

        // Aktif (henüz tamir edilmemiş) arızaları getir
        Task<List<MaintenanceDto>> GetActiveMaintenancesAsync();

        Task CreateAsync(CreateMaintenanceDto createMaintenanceDto);
        Task UpdateAsync(UpdateMaintenanceDto updateMaintenanceDto); // Tamamlandı işaretlemek için
        Task DeleteAsync(int id);
    }
}