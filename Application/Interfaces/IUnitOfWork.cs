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
        public IGenericRepository<Assignment> Assignments { get; } 
        public IGenericRepository<Department> Departments { get; } 
        public IGenericRepository<Employee> Employees { get; }
        public IGenericRepository<Intern> Interns { get; } 
        public IGenericRepository<InventoryItem> InventoryItems { get; }
        public IGenericRepository<Maintenance> Maintenances { get; } 
        public IGenericRepository<University> Universities { get; }

        Task<int> SaveChangesAsync();//int, veritabanında kaç satıra değişiklik uygulandığını döndürür
    }
}
