using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
 public class AssignmentRepository(ApplicationDbContext context) : GenericRepository<Assignment>(context), IAssignmentRepository
    { // 1. Standart Detaylı Getirme (ID'ye göre)
       public async Task<Assignment?> GetAssignmentWithDetailsAsync(int id)
        {
            return await context.Assignments
                .Where(a => !a.IsDeleted && a.Id == id)
                .Include(a => a.Employee)
                .Include(a => a.Intern)
                .Include(a => a.InventoryItem)
                .FirstOrDefaultAsync();
        }

        // 2. Tüm Zimmet Hareketlerini Listele (Admin paneli için log gibi)
       public async Task<IEnumerable<Assignment>> GetAllAssignmentsWithDetailsAsync()
        {
            return await context.Assignments
               .Where(a => !a.IsDeleted)
               .Include(a => a.Employee)
               .Include(a => a.Intern)
               .Include(a => a.InventoryItem)
               .OrderByDescending(a => a.AssignedAt)
               .ToListAsync();
        }


        // A. Bir eşyanın ŞU ANKİ (Aktif/Teslim edilmemiş) zimmetini getirir.
        // (Bir eşya aynı anda sadece 1 kişide olabilir, o yüzden tekil döner)
       public async Task<Assignment?> GetActiveAssignmentByInventoryItemIdAsync(int inventoryItemId)
        {
            return await context.Assignments
               .Where(a => !a.IsDeleted 
               && a.InventoryItemId == inventoryItemId 
               && a.ActualReturnAt==null)
               .Include(a => a.Employee)
               .Include(a => a.Intern)
               .Include(a => a.InventoryItem)
               .FirstOrDefaultAsync();
        }
        // B. Bir eşyanın GEÇMİŞ dahil tüm zimmet kayıtlarını getirir.
        // (Laptop'ın kimlerin elinden geçtiğini görmek için)
        public async Task<IEnumerable<Assignment>> GetAssignmentHistoryByInventoryItemIdAsync(int inventoryItemId)
        {
            return await context.Assignments
         .Where(a => !a.IsDeleted && a.InventoryItemId == inventoryItemId)
         .Include(a => a.Employee)
         .Include(a => a.Intern)
         .Include(a => a.InventoryItem)
         .OrderByDescending(a => a.AssignedAt)
         .ToListAsync();
        }
        // 3. Stajyere göre zimmetleri getir (Eski + Yeni hepsi)
       public async Task<IEnumerable<Assignment>> GetAssignmentsByInternIdAsync(int internId)
        {
            return await context.Assignments
         .Where(a => !a.IsDeleted && a.InternId == internId)
         .Include(a => a.Employee)
         .Include(a => a.Intern)
         .Include(a => a.InventoryItem)
         .OrderByDescending(a => a.AssignedAt)
         .ToListAsync();
        }
        // 4. Çalışana göre zimmetleri getir (Eski + Yeni hepsi)
      public async Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
        {
            return await context.Assignments
         .Where(a => !a.IsDeleted && a.EmployeeId == employeeId)
         .Include(a => a.Employee)
         .Include(a => a.Intern)
         .Include(a => a.InventoryItem)
         .OrderByDescending(a => a.AssignedAt)
         .ToListAsync();
        }

        // 5. Şirkette şu an aktif olan (Geri dönmemiş) TÜM zimmetleri listele
       public async Task<IEnumerable<Assignment>> GetActiveAssignmentsAsync()
        {
            return await context.Assignments
         .Where(a => !a.IsDeleted && a.ActualReturnAt == null)
         .Include(a => a.Employee)
         .Include(a => a.Intern)
         .Include(a => a.InventoryItem)
         .OrderByDescending(a => a.AssignedAt)
         .ToListAsync();
        }

    }
}
