using Envanter_Takip_Projesi.Extensions;
using Envanter_Takip_Projesi.Middlewares;
using Infrastructure;
using Application;
using Hangfire; // Application servislerini eklemek için

namespace Envanter_Takip_Projesi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // OpenAPI (Swagger altyapısı)
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer(); // Controller'ları Swagger'ın bulması için şart!

            // --- KATMAN SERVİSLERİMİZ ---
            // Infrastructure (Veritabanı, UnitOfWork vb.)
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Auth (JWT Ayarları vb.)
            builder.Services.AddAuthServices(builder.Configuration);

            // Swagger Ayarları (Senin yazdığın extension)
            builder.Services.AddSwaggerServices();

            // YENİ EKLENEN: Bizim yazdığımız Application katmanı servisleri (Mapper ve Servisler)
            builder.Services.AddApplicationServices();
            // -----------------------------

            var app = builder.Build();

            // 1. Önce Hata Yakalayıcı (Tüm hatalar burada filtrelenir)
            app.UseCustomExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // YENİ EKLENEN: Swagger görsel arayüzünü (UI) tarayıcıda göstermek için
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseHangfireDashboard();
            app.UseCors("AllowAll");

            // YENİ EKLENEN: Kapıdaki güvenlik görevlisi (Token var mı yok mu?)
            app.UseAuthentication();

            // Kimlik kontrolünden geçen kişinin yetkisi var mı? (Admin mi, User mı?)
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}