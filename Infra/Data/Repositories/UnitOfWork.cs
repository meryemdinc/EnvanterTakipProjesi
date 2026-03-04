using Application.Interfaces;
using Application.Interfaces.Repositories;


namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        // Interface'e (Menüye), Concrete Class'ı (Mutfağı) atıyoruz ve context'i içine yolluyoruz.
        public IAppUserRepository AppUsers { get; } = new AppUserRepository(context);
        public IAssignmentRepository Assignments { get; } = new AssignmentRepository(context);
        public IDepartmentRepository Departments { get; } = new DepartmentRepository(context);
        public IEmployeeRepository Employees { get; } = new EmployeeRepository(context);
        public IInternRepository Interns { get; } = new InternRepository(context);
        public IInventoryItemRepository InventoryItems { get; } = new InventoryItemRepository(context);
        public IMaintenanceRepository Maintenances { get; } = new MaintenanceRepository(context);
        public IUniversityRepository Universities { get; } = new UniversityRepository(context);

        public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

        public void Dispose()
        {
            context.Dispose();
        }
    }
}