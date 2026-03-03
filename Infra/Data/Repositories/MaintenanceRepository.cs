using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class MaintenanceRepository(ApplicationDbContext context) : GenericRepository<Maintenance>(context), IMaintenanceRepository
    {
        // 1. DETAYLI GETİR (Update yapılabilir -> Tracking AÇIK)
        public async Task<Maintenance?> GetMaintenanceWithDetailsAsync(int id)
        {
            return await context.Maintenances
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.InventoryItem) // Hangi eşya bozulmuş görelim
                .FirstOrDefaultAsync();
        }

        // 2. TÜM BAKIMLAR (Raporlama -> Tracking KAPALI )
        public async Task<IEnumerable<Maintenance>> GetAllMaintenancesWithDetailsAsync()
        {
            return await context.Maintenances
                .AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Include(m => m.InventoryItem)
                .OrderByDescending(m => m.ReportedAt) // En son bozulan en üstte
                .ToListAsync();
        }

        // 3. EŞYA BAZLI GEÇMİŞ (Tarihçe -> Tracking KAPALI )
        public async Task<IEnumerable<Maintenance>> GetMaintenancesByInventoryItemIdAsync(int inventoryItemId)
        {
            return await context.Maintenances
                .AsNoTracking()
                .Where(m => !m.IsDeleted && m.InventoryItemId == inventoryItemId)
                .Include(m => m.InventoryItem)
                .OrderByDescending(m => m.ReportedAt)
                .ToListAsync();
        }

        // 4. AKTİF BAKIMLAR (Tamir Tarihi NULL olanlar -> Tracking KAPALI )
        public async Task<IEnumerable<Maintenance>> GetActiveMaintenancesAsync()
        {
            return await context.Maintenances
                .AsNoTracking()
                .Where(m => !m.IsDeleted && m.RepairedAt == null) // Henüz tamir edilmemiş
                .Include(m => m.InventoryItem)
                .OrderByDescending(m => m.ReportedAt)
                .ToListAsync();
        }

        // 5. TARİH ARALIĞI (Maliyet Raporu vb. -> Tracking KAPALI )
        public async Task<IEnumerable<Maintenance>> GetMaintenancesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await context.Maintenances
                .AsNoTracking()
                .Where(m => !m.IsDeleted &&
                            m.ReportedAt >= startDate &&
                            m.ReportedAt <= endDate)
                .Include(m => m.InventoryItem)
                .OrderByDescending(m => m.ReportedAt)
                .ToListAsync();
        }
    }
}