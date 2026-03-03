using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IMaintenanceRepository : IGenericRepository<Maintenance>
    {
        // 1. Bakım kaydını, eşya bilgisiyle detaylı getir
        Task<Maintenance?> GetMaintenanceWithDetailsAsync(int id);

        // 2. Tüm bakım kayıtlarını listele (Genel Rapor)
        Task<IEnumerable<Maintenance>> GetAllMaintenancesWithDetailsAsync();

        // 3. Bir eşyanın bakım geçmişini getir (Bu cihaz daha önce kaç kere bozuldu?)
        Task<IEnumerable<Maintenance>> GetMaintenancesByInventoryItemIdAsync(int inventoryItemId);

        // 4. Şu an tamirde olan (Henüz tamir edilmemiş) kayıtları getir
        Task<IEnumerable<Maintenance>> GetActiveMaintenancesAsync();

        // 5. Belirli bir tarih aralığındaki bakımları getir (Maliyet raporu için)
        Task<IEnumerable<Maintenance>> GetMaintenancesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}