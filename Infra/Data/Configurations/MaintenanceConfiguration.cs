using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class MaintenanceConfiguration:IEntityTypeConfiguration<Maintenance>
    {
        public void Configure(EntityTypeBuilder<Maintenance> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.CreatedAt).IsRequired();
            builder.Property(m => m.IsDeleted).HasDefaultValue(false);
            builder.Property(m => m.Description).HasMaxLength(500);
            builder.Property(m => m.PerformedBy).HasMaxLength(50);
            builder.Property(m => m.ReportedAt).IsRequired();
            builder.Property(m => m.Cost).HasPrecision(18, 2);
            builder.HasQueryFilter(m => !m.IsDeleted);
        }

    }
}
