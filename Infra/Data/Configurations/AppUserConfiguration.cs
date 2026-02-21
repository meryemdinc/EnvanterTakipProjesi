using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
   public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        { 
        builder.HasKey(u => u.Id);
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.IsDeleted).HasDefaultValue(false);//BaseEntitydeki defaul value olarak false atanması ile AppUser nesnesi oluşturulurken IsDeleted property'sinin
                                                                      //default olarak false gelmesi sağlanır.configurationdaki bu ayar ile veritabanında manuel olarak AppUser nesnesi eklenmek istendiğinde(INSERT INTO) lazımdır
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u=>u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired().HasMaxLength(20).HasDefaultValue("User");
            builder.HasOne(u=>u.Employee)
                   .WithOne(x=>x.AppUser)
                   .HasForeignKey<Employee>(x => x.AppUserId)//One-to-one ilişki için <Employee> diye foreign key nin hangi tabloya yazılacağını belirliyoruz
                   .OnDelete(DeleteBehavior.SetNull);//AppUser silindiğinde Employee tablosundaki AppUserId foreign key null olarak güncellenir, böylece Employee kaydı korunur ancak AppUser ile ilişkisi kesilir.
        }
    }
}
