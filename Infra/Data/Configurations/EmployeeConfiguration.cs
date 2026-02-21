using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
   public class EmployeeConfiguration:IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);

            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Ignore(e => e.FullName);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Phone).IsRequired().HasMaxLength(10);//5xx xxx xx xx formatında telefon numarası
            builder.Property(e => e.Role).IsRequired().HasMaxLength(50);
            builder.Property(e => e.StartDate).IsRequired();

            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasOne(e => e.AppUser)
           .WithOne(u => u.Employee)
           .HasForeignKey<Employee>(e => e.AppUserId)
           .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
