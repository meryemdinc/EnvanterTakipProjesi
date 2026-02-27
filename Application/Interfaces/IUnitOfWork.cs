using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork: IDisposable 
    {
       // Bütün repolara tek bir noktadan(tek bir referansla) ulaşmak
       // için Unit Of Work yazıyoruz.
         IGenericRepository<AppUser> AppUsers { get; } //set yazmıyorux cünkü repository'ler sadece get
                                                       //ile erişilecek, dışarıdan set ile değiştirilmemesi için
         IGenericRepository<Assignment> Assignments { get; } 
        IGenericRepository<Department> Departments { get; } 
         IGenericRepository<Employee> Employees { get; }
         IInternRepository Interns { get; } 
        IGenericRepository<InventoryItem> InventoryItems { get; }
       IGenericRepository<Maintenance> Maintenances { get; } 
        IGenericRepository<University> Universities { get; }

        Task<int> SaveChangesAsync();//int, veritabanında kaç satıra değişiklik uygulandığını döndürür
    }
}
