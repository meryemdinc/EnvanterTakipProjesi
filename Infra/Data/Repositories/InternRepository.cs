using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class InternRepository(ApplicationDbContext context) : GenericRepository<Intern>(context), IInternRepository
    {
        // 1. TEKİL GETİR
        public async Task<Intern?> GetInternWithDetailsAsync(int id)
        {
            return await context.Interns
                .Where(x => !x.IsDeleted)
                .Include(i => i.University)
                .Include(i => i.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // 2. TÜMÜNÜ GETİR
        public async Task<IEnumerable<Intern>> GetAllInternsWithDetailsAsync()
        {
            return await context.Interns
                .Where(x => !x.IsDeleted)
                .Include(i => i.University)
                .Include(i => i.Assignments)
                .ToListAsync();
        }

        // 3. ÜNİVERSİTEYE GÖRE GETİR
        public async Task<IEnumerable<Intern>> GetInternsByUniversityAsync(int universityId)
        {
            // İki where yerine tek where ile birleştirdik (Clean Code)
            return await context.Interns
                .Where(x => !x.IsDeleted && x.UniversityId == universityId)
                .Include(i => i.University)
                .ToListAsync();
        }

        // 4. AKTİF STAJYERLER
        public async Task<IEnumerable<Intern>> GetActiveInternsAsync()
        {
            var today = DateTime.UtcNow;
            return await context.Interns
                .Where(i => !i.IsDeleted &&
                            i.StartDate <= today &&
                            i.EndDate >= today)
                .Include(i => i.University)
                .Include(i => i.Assignments)
                .ToListAsync();
        }
    }
}