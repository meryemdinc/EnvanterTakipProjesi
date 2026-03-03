using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork: IDisposable 
    {
       // Bütün repolara tek bir noktadan(tek bir referansla) ulaşmak
       // için Unit Of Work yazıyoruz.
         IAppUserRepository AppUsers { get; } //set yazmıyorux cünkü repository'ler sadece get
                                                       //ile erişilecek, dışarıdan set ile değiştirilmemesi için
         IAssignmentRepository Assignments { get; } 
        IDepartmentRepository Departments { get; } 
         IEmployeeRepository Employees { get; }
         IInternRepository Interns { get; } 
        IInventoryItemRepository InventoryItems { get; }
       IMaintenanceRepository Maintenances { get; } 
        IGenericRepository<University> Universities { get; }

        Task<int> SaveChangesAsync();//int, veritabanında kaç satıra değişiklik uygulandığını döndürür
    }
}
