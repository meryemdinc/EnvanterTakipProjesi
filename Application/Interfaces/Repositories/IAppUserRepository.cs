using Domain.Entities;

namespace Application.Interfaces.Repositories
{
   public interface IAppUserRepository:IGenericRepository<AppUser>
    {
        // 1. ID ile detaylı getir (Employee bilgisiyle birlikte)
        Task<AppUser?> GetAppUserWithDetailsAsync(int id);

        // 2. Tüm kullanıcıları detaylı listele
        Task<IEnumerable<AppUser>> GetAllAppUsersWithDetailsAsync();

        // 3. Email ile TEK bir kullanıcı bul (Login işlemleri için şart!)
        // DİKKAT: IEnumerable değil, tek AppUser dönüyoruz. Parametre string.
        Task<AppUser?> GetByEmailAsync(string email);

        // 4. Belirli bir role sahip kullanıcıları getir (Örn: Sadece Adminler)
        Task<IEnumerable<AppUser>> GetUsersByRoleAsync(string role);

        // 5. Aktif (Silinmemiş) Kullanıcılar
        Task<IEnumerable<AppUser>> GetActiveAppUsersAsync();
    }
}
