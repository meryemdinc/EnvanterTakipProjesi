using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Data.Repositories
{
    public class AppUserRepository(ApplicationDbContext context): GenericRepository<AppUser>(context),IAppUserRepository
    {
       public async Task<AppUser?> GetAppUserWithDetailsAsync(int id) {
            return await context.AppUsers
                .Where(user => !user.IsDeleted && user.Id == id)
                .Include(user => user.Employee)
                .FirstOrDefaultAsync();
        }

        // 2. Tüm kullanıcıları detaylı listele
       public async Task<IEnumerable<AppUser>> GetAllAppUsersWithDetailsAsync() {
            return await context.AppUsers
                 .Where(user => !user.IsDeleted)
                 .Include(user => user.Employee)
                 .ToListAsync();
        }

        // 3. Email ile TEK bir kullanıcı bul (Login işlemleri için şart!)
        // DİKKAT: IEnumerable değil, tek AppUser dönüyoruz. Parametre string.
        //equals(email) deseydik büyük küçük harf duyarlılığı olurdu, bu yüzden == kullandık.
        public async Task<AppUser?> GetByEmailAsync(string email) {
            return await context.AppUsers
                .Where(user => !user.IsDeleted && user.Email == email)
                .Include(user => user.Employee)
                .FirstOrDefaultAsync();
        }

        // 4. Belirli bir role sahip kullanıcıları getir (Örn: Sadece Adminler)
       public async Task<IEnumerable<AppUser>> GetUsersByRoleAsync(string role) {
            return await context.AppUsers
                  .Where(user => !user.IsDeleted && user.Role == role)
                  .Include(user => user.Employee)
                  .ToListAsync();
        }

        // 5. Aktif (Silinmemiş) Kullanıcılar,eager loading ile Employee bilgisi de geliyor.
        public async Task<IEnumerable<AppUser>> GetActiveAppUsersAsync() {
            return await context.AppUsers
                    .Where(user => !user.IsDeleted )
                    .Include(user => user.Employee)
                    .ToListAsync();
        }
    }
}
