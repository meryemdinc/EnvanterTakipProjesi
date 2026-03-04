using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        // 1. Üniversiteyi, okuyan stajyer listesiyle birlikte getir
        Task<University?> GetUniversityWithStudentsAsync(int id);

        // 2. Tüm üniversiteleri listele (Alfabetik)
        Task<IEnumerable<University>> GetAllUniversitiesWithDetailsAsync();

        // 3. Şehre göre üniversiteleri getir (Örn: "Ankara"daki üniversiteler)
        Task<IEnumerable<University>> GetUniversitiesByCityAsync(string city);

        // 4. İsme göre bul (Aynı isimde üniversite eklememek için validasyon)
        Task<University?> GetByNameAsync(string name);
    }
}