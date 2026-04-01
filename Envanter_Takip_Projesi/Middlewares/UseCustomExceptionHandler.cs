using Application.DTOs.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace Envanter_Takip_Projesi.Middlewares
{
 
    public static class CustomExceptionMiddlewareExtensions
    {
        // 2. Parametre tipi evrensel standart olan IApplicationBuilder yapıldı
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            // this IApplicationBuilder anlamı:
            // Ey C#, benim yazdığım bu metodu al, Microsoft'un kendi yazdığı
            // orijinal IApplicationBuilder sınıfının içine gizlice monte et (Extension Method).

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        var errorMessage = exceptionHandlerPathFeature.Error.Message;

                        //  HTTP Status Code'u 500 yapıyoruz
                        context.Response.StatusCode = 500;

                        // Zarfımızı oluşturuyoruz,hata mesajı InternDto,InventoryItemDto döndürmez
                       // sadece hata mesajı içerir data yok o yuzden T'yi NoContent yapıyoruz

                        var response = Response<NoContent>.Fail(errorMessage, 500, true);

                        // JSON olarak Frontend'e fırlatıyoruz
                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });
        }
    }
}