using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        // 1. Standart Detaylı Getirme (ID'ye göre)
        Task<Assignment?> GetAssignmentWithDetailsAsync(int id);

        // 2. Tüm Zimmet Hareketlerini Listele (Admin paneli için log gibi)
        Task<IEnumerable<Assignment>> GetAllAssignmentsWithDetailsAsync();


        // A. Bir eşyanın ŞU ANKİ (Aktif/Teslim edilmemiş) zimmetini getirir.
        // (Bir eşya aynı anda sadece 1 kişide olabilir, o yüzden tekil döner)
        Task<Assignment?> GetActiveAssignmentByInventoryItemIdAsync(int inventoryItemId);

        // B. Bir eşyanın GEÇMİŞ dahil tüm zimmet kayıtlarını getirir.
        // (Laptop'ın kimlerin elinden geçtiğini görmek için)
        Task<IEnumerable<Assignment>> GetAssignmentHistoryByInventoryItemIdAsync(int inventoryItemId);

        // 3. Stajyere göre zimmetleri getir (Eski + Yeni hepsi)
        Task<IEnumerable<Assignment>> GetAssignmentsByInternIdAsync(int internId);

        // 4. Çalışana göre zimmetleri getir (Eski + Yeni hepsi)
        Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeIdAsync(int employeeId);

        // 5. Şirkette şu an aktif olan (Geri dönmemiş) TÜM zimmetleri listele
        Task<IEnumerable<Assignment>> GetActiveAssignmentsAsync();
    }
}