using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        // Interface'e (Menüye), Concrete Class'ı (Mutfağı) atıyoruz ve context'i içine yolluyoruz.
        public IGenericRepository<AppUser> AppUsers { get; } = new GenericRepository<AppUser>(context);
        public IGenericRepository<Assignment> Assignments { get; } = new GenericRepository<Assignment>(context);
        public IGenericRepository<Department> Departments { get; } = new GenericRepository<Department>(context);
        public IGenericRepository<Employee> Employees { get; } = new GenericRepository<Employee>(context);
        public IGenericRepository<Intern> Interns { get; } = new GenericRepository<Intern>(context);
        public IGenericRepository<InventoryItem> InventoryItems { get; } = new GenericRepository<InventoryItem>(context);
        public IGenericRepository<Maintenance> Maintenances { get; } = new GenericRepository<Maintenance>(context);
        public IGenericRepository<University> Universities { get; } = new GenericRepository<University>(context);

        public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

        public void Dispose()
        {
            context.Dispose();
        }
    }
}