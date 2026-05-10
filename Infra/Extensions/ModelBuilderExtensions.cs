using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions
{

    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalQueryFilters(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Assignment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Intern>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<InventoryItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Maintenance>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<University>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}