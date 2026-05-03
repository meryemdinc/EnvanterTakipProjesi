using Application.DTOs.Common;
using Application.Exceptions; 
using Microsoft.AspNetCore.Diagnostics;

namespace Envanter_Takip_Projesi.Middlewares
{
    public static class CustomExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionFeature?.Error != null)
                    {
                        // 1. Düşen hatayı (Exception) yakaladık
                        var exception = exceptionFeature.Error;

                        // 2. Hatamızın türüne göre HTTP Durum Kodunu (Status Code) belirliyoruz
                        // C#'ın harika Switch Expression yeteneği!
                        int statusCode = exception switch
                        {
                            NotFoundException => 404, // Bulunamadı
                            ItemNotAvailableException => 400, // Kötü İstek (Zaten kullanımda vb.)
                            DuplicateCategoryAssignmentException => 400, // Kötü İstek (Çifte laptop)
                            AssignmentConflictException => 409, // Çakışma (Tarihler uyuşmuyor)
                            BadRequestException => 400,
                            _ => 500 // Yukarıdakilerin hiçbiri değilse (Beklenmedik bir C# hatasıysa) Sunucu Hatası dön
                        };

                        // 3. İsteğin gidişatını (Header) bu yeni koda göre ayarlıyoruz
                        context.Response.StatusCode = statusCode;

                        // 4. Standart zarfımızı oluşturuyoruz. Hata mesajını Exception'ın kendi içinden alıyoruz.
                        var response = Response<NoContent>.Fail(exception.Message, statusCode, true);

                        // 5. JSON olarak Frontend'e fırlatıyoruz
                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });
        }
    }
}