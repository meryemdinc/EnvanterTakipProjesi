using Domain.Entities;
using Domain.Common; 
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Intern> Interns { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Maintenance> Maintenances { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        //global soft delete filter
     modelBuilder.ApplyGlobalQueryFilters();
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
   
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
           
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow; 
            }
         
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow; 
            }
        }

     
        return base.SaveChangesAsync(cancellationToken);
    }
}