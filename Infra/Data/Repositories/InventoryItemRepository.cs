using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class InventoryItemRepository(ApplicationDbContext context) : GenericRepository<InventoryItem>(context), IInventoryItemRepository
    {
        // 1. DETAYLI GETİR (Zimmet ve Bakım geçmişi dahil - Tracking AÇIK)
        public async Task<InventoryItem?> GetItemWithDetailsAsync(int id)
        {
            return await context.InventoryItems
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.Assignments)
                    .ThenInclude(a => a.Employee) // Eşya kimlere verilmiş ismen görelim
                .Include(x => x.Assignments)
                    .ThenInclude(a => a.Intern)   // Veya hangi stajyere verilmiş
                .Include(x => x.Maintenances)     // Bakım kayıtları
                .FirstOrDefaultAsync();
        }

        // 2. TÜM ENVANTER (Listeleme - Tracking KAPALI )
        public async Task<IEnumerable<InventoryItem>> GetAllItemsWithDetailsAsync()
        {
            return await context.InventoryItems
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Include(x => x.Assignments) // Sadece zimmet var mı diye bakmak için
                .OrderBy(x => x.ItemCode)    // Demirbaş koduna göre sıralı
                .ToListAsync();
        }

        // 3. KATEGORİYE GÖRE FİLTRELE (Tracking KAPALI )
        public async Task<IEnumerable<InventoryItem>> GetItemsByCategoryAsync(string category)
        {
            var normalizedCategory = category.ToLower();

            return await context.InventoryItems
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Category.ToLower() == normalizedCategory)
                .OrderBy(x => x.Model)
                .ToListAsync();
        }

        // 4. STATÜYE GÖRE (Örn: Depodakileri getir)
        public async Task<IEnumerable<InventoryItem>> GetItemsByStatusAsync(ItemStatus status)
        {
            return await context.InventoryItems
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Status == status)
                .OrderBy(x => x.ItemCode)
                .ToListAsync();
        }


        // 5. DEMİRBAŞ KODU İLE BUL (Tekil Kayıt)
        public async Task<InventoryItem?> GetByItemCodeAsync(string itemCode)
        {
            var normalizedCode = itemCode.ToLower();

            return await context.InventoryItems
                .Where(x => !x.IsDeleted && x.ItemCode.ToLower() == normalizedCode)
                .FirstOrDefaultAsync();
        }

        // 6. GARANTİ RAPORU (Örn: Garantisi bitmişleri listele)
        public async Task<IEnumerable<InventoryItem>> GetItemsWithExpiringWarrantyAsync(DateTime thresholdDate)
        {
            return await context.InventoryItems
                .AsNoTracking()
                .Where(x => !x.IsDeleted &&
                            x.WarrantyEndDate != null &&
                            x.WarrantyEndDate <= thresholdDate) // Belirtilen tarihten önce bitenler
                .OrderBy(x => x.WarrantyEndDate) // En acil olan en üstte
                .ToListAsync();
        }
    }
}