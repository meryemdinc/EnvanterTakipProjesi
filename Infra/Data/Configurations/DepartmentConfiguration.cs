using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
  public class DepartmentConfiguration:IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.CreatedAt).IsRequired();
            builder.Property(d => d.IsDeleted).HasDefaultValue(false);

            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(d => d.Name).IsUnique();
            builder.HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
          new Department { Id = 1, Name = "Frontend", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
          new Department { Id = 2, Name = "Backend", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
          new Department { Id = 3, Name = "Mobile", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
          new Department { Id = 4, Name = "Database", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
          new Department { Id = 5, Name = "Fullstack", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
      );
        }
    }
}
