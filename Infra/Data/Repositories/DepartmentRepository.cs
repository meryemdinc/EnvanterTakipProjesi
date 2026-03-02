using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) : GenericRepository<Department>(context), IDepartmentRepository
    {
        // 1. Departmanı, içindeki çalışan listesiyle birlikte getir
        public async Task<Department?> GetDepartmentWithEmployeesAsync(int id)
        {
            return await context.Departments
                .Where(d => !d.IsDeleted && d.Id == id)
                .Include(d => d.Employees)
                .FirstOrDefaultAsync();
        }

        // 2. Tüm departmanları, çalışanlarıyla birlikte listele
       public async Task<IEnumerable<Department>> GetAllDepartmentsWithEmployeesAsync()
        {
            return await context.Departments
                   .AsNoTracking()
              .Where(d => !d.IsDeleted)
               .Include(d => d.Employees)
            .OrderBy(d => d.Name)
              .ToListAsync();
        }

        // 3. İsme göre departman bul (Validasyon için: Aynı isimde departman var mı?)
       public async Task<Department?> GetByNameAsync(string name)
        {
            return await context.Departments
                   .AsNoTracking()
                .Where(d => !d.IsDeleted && d.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }
    }
}
