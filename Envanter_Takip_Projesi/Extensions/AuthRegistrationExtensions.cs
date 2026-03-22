using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//JWT ile ilgili servisleri merkezi bir yerde kaydetmek ve yönetmek için kullanılan sınıf. Program.cs de tek satırda ekleyeceğiz
//program.cste bir satır yazmak yerine bu class ı oluşturuyoruz ve tek satırda program.cs e ekliyoruz.single responsibility kuralını uygulamış oluyoruz.

namespace Envanter_Takip_Projesi.Extensions
{
    public static class AuthRegistrationExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSetting:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!))
                    };

                });
            return services;
        }
    }
}
