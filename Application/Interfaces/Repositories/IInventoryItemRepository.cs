using Domain.Entities;
using Domain.Enums; // ItemStatus enum'ı için gerekli

namespace Application.Interfaces.Repositories
{
    public interface IInventoryItemRepository : IGenericRepository<InventoryItem>
    {
        // 1. Eşyayı detaylı getir (Zimmet ve Bakım geçmişiyle)
        Task<InventoryItem?> GetItemWithDetailsAsync(int id);

        // 2. Tüm envanteri listele
        Task<IEnumerable<InventoryItem>> GetAllItemsWithDetailsAsync();

        // 3. Kategoriye göre getir (Örn: "Laptop", "Monitor")
        Task<IEnumerable<InventoryItem>> GetItemsByCategoryAsync(string category);

        // 4. Duruma göre getir (Örn: Sadece "Available" - Boşta olanlar)
        Task<IEnumerable<InventoryItem>> GetItemsByStatusAsync(ItemStatus status);


        // 5. Demirbaş Koduna (ItemCode) göre bul
        Task<InventoryItem?> GetByItemCodeAsync(string itemCode);

        // 6. Garantisi yaklaşan veya bitmiş ürünleri getir (Raporlama için)
        // thresholdDate: Hangi tarihten öncesi? (Örn: Bugün)
        Task<IEnumerable<InventoryItem>> GetItemsWithExpiringWarrantyAsync(DateTime thresholdDate);
    }
}