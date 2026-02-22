using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(ii => ii.Id);
            builder.Property(ii => ii.CreatedAt).IsRequired();
            builder.Property(ii => ii.IsDeleted).HasDefaultValue(false);

            builder.Property(ii => ii.ItemCode).IsRequired().HasMaxLength(100);
            builder.HasIndex(ii => ii.ItemCode).IsUnique();
            builder.Property(ii => ii.SerialNumber).HasMaxLength(50);
            builder.Property(ii => ii.Category).IsRequired().HasMaxLength(50);
            builder.Property(ii => ii.Brand).HasMaxLength(50);
            builder.Property(ii => ii.Model).HasMaxLength(100);
            builder.Property(ii => ii.Notes).HasMaxLength(100);
            builder.Property(ii => ii.Status)
                     .HasConversion<string>()
                     .HasMaxLength(50)
                     .IsRequired()
                     .HasDefaultValue(ItemStatus.Available);
            builder.HasMany(ii => ii.Maintenances)
                .WithOne(m => m.InventoryItem)
                .HasForeignKey(m=> m.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(ii => !ii.IsDeleted);
            builder.HasIndex(ii => ii.SerialNumber).IsUnique();
            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
    }
}

