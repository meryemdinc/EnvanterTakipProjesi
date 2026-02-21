using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Configurations
{
   public class AssignmentConfiguration:IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint("CK_Assignments_Assignee",
                "(\"InternId\" IS NULL OR \"EmployeeId\" IS NULL)"));

            builder.HasKey(a => a.Id);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.IsDeleted).HasDefaultValue(false);
            builder.Property(a => a.AssignedAt).IsRequired();
            builder.Property(a => a.Notes).HasMaxLength(500);

            builder.HasOne(a => a.InventoryItem)
                   .WithMany(i => i.Assignments)
                   .HasForeignKey(a => a.InventoryItemId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Intern)
                .WithMany(i => i.Assignments)
                .HasForeignKey(a => a.InternId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Employee)
                .WithMany(i => i.Assignments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}