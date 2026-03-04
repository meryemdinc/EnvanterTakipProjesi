using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UniversityRepository(ApplicationDbContext context) : GenericRepository<University>(context), IUniversityRepository
    {
        // 1. DETAYLI GETİR (Update ihtimali -> Tracking AÇIK)
        public async Task<University?> GetUniversityWithStudentsAsync(int id)
        {
            return await context.Universities
                .Where(u => !u.IsDeleted && u.Id == id)
                .Include(u => u.Students) // O üniversitedeki stajyerleri de görelim
                .FirstOrDefaultAsync();
        }

        // 2. TÜM LİSTE (Sadece okuma -> Tracking KAPALI )
        public async Task<IEnumerable<University>> GetAllUniversitiesWithDetailsAsync()
        {
            return await context.Universities
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .Include(u => u.Students) // İstatistik için (Örn: ODTÜ - 5 Stajyer)
                .OrderBy(u => u.Name)     // Alfabetik sıralama (A-Z)
                .ToListAsync();
        }

        // 3. ŞEHRE GÖRE (Filtreleme -> Tracking KAPALI )
        public async Task<IEnumerable<University>> GetUniversitiesByCityAsync(string city)
        {
            var normalizedCity = city.ToLower();

            return await context.Universities
                .AsNoTracking()
                .Where(u => !u.IsDeleted &&
                            u.City != null &&
                            u.City.ToLower() == normalizedCity)
                .Include(u => u.Students)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // 4. İSME GÖRE BUL (Validasyon -> Tracking KAPALI )
        public async Task<University?> GetByNameAsync(string name)
        {
            var normalizedName = name.ToLower();

            return await context.Universities
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Name.ToLower() == normalizedName)
                .FirstOrDefaultAsync();
        }
    }
}