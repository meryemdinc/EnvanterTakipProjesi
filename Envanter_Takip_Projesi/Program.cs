using Envanter_Takip_Projesi.API.Extensions;
using Envanter_Takip_Projesi.Extensions;
using Envanter_Takip_Projesi.Middlewares;
using Infrastructure;
namespace Envanter_Takip_Projesi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructureServices(builder.Configuration);//infrastructure katmanındaki servisleri ekliyoruz
            builder.Services.AddAuthServices(builder.Configuration);//auth işlemleri için gerekli servisleri ekliyoruz
            builder.Services.AddSwaggerServices();

            var app = builder.Build();

            app.UseCustomExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
