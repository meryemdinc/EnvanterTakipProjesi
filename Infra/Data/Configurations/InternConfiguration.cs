using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
   public class InternConfiguration: IEntityTypeConfiguration<Intern>
    {
        public void Configure(EntityTypeBuilder<Intern> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.CreatedAt).IsRequired();
            builder.Property(i => i.IsDeleted).HasDefaultValue(false);
            builder.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.LastName).IsRequired().HasMaxLength(50);
            builder.Ignore(i => i.FullName);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(i => i.Email).IsUnique();
            builder.Property(i => i.Phone).IsRequired().HasMaxLength(10);//5xx xxx xx xx formatında telefon numarası
            builder.Property(i => i.StartDate).IsRequired();
            builder.Property(i => i.EndDate).IsRequired();
            builder.Property(i => i.WorkDaysPerWeek).IsRequired().HasDefaultValue(5);
            builder.HasOne(i => i.University)
                .WithMany(u => u.Students)
                .HasForeignKey(i => i.UniversityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasQueryFilter(i => !i.IsDeleted);
            builder.Property(i => i.Degree).IsRequired();
            builder.Property(i => i.ClassYear).IsRequired();
            builder.Property(i => i.StudentNumber).HasMaxLength(50);

            builder.ToTable(t => t.HasCheckConstraint("CK_Intern_WorkDays", "\"WorkDaysPerWeek\" >= 1 AND \"WorkDaysPerWeek\" <= 5"));
        }
    }
}
