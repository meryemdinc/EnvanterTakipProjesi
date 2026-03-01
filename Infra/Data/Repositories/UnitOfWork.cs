using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        // Interface'e (Menüye), Concrete Class'ı (Mutfağı) atıyoruz ve context'i içine yolluyoruz.
        public IAppUserRepository AppUsers { get; } = new AppUserRepository(context);
        public IAssignmentRepository Assignments { get; } = new AssignmentRepository(context);
        public IGenericRepository<Department> Departments { get; } = new GenericRepository<Department>(context);
        public IGenericRepository<Employee> Employees { get; } = new GenericRepository<Employee>(context);
        public IInternRepository Interns { get; } = new InternRepository(context);
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