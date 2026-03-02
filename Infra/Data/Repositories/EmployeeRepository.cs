using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context) : GenericRepository<Employee>(context), IEmployeeRepository
    {
        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await context.Employees
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.Department)      // Hangi departmanda?
                .Include(e => e.AppUser)         // Sisteme giriş yetkisi var mı?
                .Include(e => e.Assignments)     // Üzerinde zimmetli ne var? (Önemli!)
                    .ThenInclude(a => a.InventoryItem) // O zimmetlerin detayını da görelim
                .FirstOrDefaultAsync();
        }

        // 2. GENEL LİSTE (Sadece okuma - Tracking KAPALI )
        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            return await context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Department)
                .Include(e => e.AppUser)
                // Not: Listede Assignments'ı çekmek veriyi şişirebilir, detayda çekmek daha iyidir.
                // Ama proje küçükse burada da çekilebilir. Şimdilik sadece ana bilgileri aldık.
                .OrderBy(e => e.FirstName) // İsim sırasına göre gelsin
                .ToListAsync();
        }

        // 3. DEPARTMANA GÖRE (Tracking KAPALI )
        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted && e.DepartmentId == departmentId)
                .Include(e => e.Department)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        // 4. EMAİL İLE BUL (Validasyon için - Tracking KAPALI9
        public async Task<Employee?> GetByEmailAsync(string email)
        {
            // Büyük/Küçük harf duyarsızlığı için
            var normalizedEmail = email.ToLower();

            return await context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted && e.Email.ToLower() == normalizedEmail)
                .Include(e => e.Department)
                .FirstOrDefaultAsync();
        }

        // 5. AKTİF ÇALIŞANLAR 
        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await context.Employees
                .AsNoTracking()
                .Where(e => !e.IsDeleted &&
                           (e.EndDate == null || e.EndDate >= today)) // Çıkış tarihi yoksa VEYA gelecekteyse
                .Include(e => e.Department)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }
    }
}