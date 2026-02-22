using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.City).HasMaxLength(50);


        builder.HasData(
            new University { Id = 1, Name = "Eskişehir Osmangazi Üniversitesi", City = "Eskişehir", CreatedAt = DateTime.UtcNow },
            new University { Id = 2, Name = "Orta Doğu Teknik Üniversitesi", City = "Ankara", CreatedAt = DateTime.UtcNow },
            new University { Id = 3, Name = "Boğaziçi Üniversitesi", City = "İstanbul", CreatedAt = DateTime.UtcNow },
            new University { Id = 4, Name = "İstanbul Teknik Üniversitesi", City = "İstanbul", CreatedAt = DateTime.UtcNow }
        );
    }
}

