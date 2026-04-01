using Microsoft.OpenApi.Models;

namespace Envanter_Takip_Projesi.Extensions { 
    public static class SwaggerRegistrationExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
             
                // Swagger'a "Biz Bearer adında, Header üzerinden taşınan bir API Key mantığı kullanıyoruz" diyoruz.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization başlığı (Header) Bearer şemasını kullanır.\r\n\r\nAşağıdaki kutuya 'Bearer' yazıp boşluk bıraktıktan sonra Token'ınızı yapıştırın.\r\n\r\nÖrnek: \"Bearer eyJhbGci...\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

             
                // Swagger'a "Yukarıda tanımladığım 'Bearer' şemasını projedeki tüm uçlara (endpoint) kilitle" diyoruz.
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>() // Kapsam (Scope) boş bırakılır, tüm API için geçerlidir
                    }
                });
            });

            return services;
        }
    }
}