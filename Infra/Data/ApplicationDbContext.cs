using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)//primary constructor(.Net 8 ile gelen özellik)
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
            base.OnModelCreating(modelBuilder);
        }
    }

