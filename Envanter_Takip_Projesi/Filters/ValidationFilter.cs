using Application.DTOs.Common; // Response, ErrorDto ve NoContent sınıflarının olduğu yer
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Envanter_Takip_Projesi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Kurallarımızdan geçen hatalı bir durum var mı?
            if (!context.ModelState.IsValid)
            {
                // 2. ModelState havuzundaki tüm hata mesajlarını toplayıp bir string listesi yapıyoruz.
                var errors = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                // 3. Senin ErrorDto nesneni oluşturuyoruz.
                // Validasyon hataları (Örn: "Şifre kısa", "Ad boş") her zaman kullanıcıya gösterilmelidir.
                // Bu yüzden IsShow parametresini 'true' olarak ayarlıyoruz!
                var errorDto = new ErrorDto(errors, true);

                // 4. Kendi standart zarfımızı (Response) Fail metoduyla hazırlıyoruz.
                // Data kısmı boş olacağı için NoContent struct'ını kullanıyoruz.
                var response = Response<NoContent>.Fail(errorDto, 400);

                // 5. HTTP 400 (Bad Request) durum koduyla birlikte zarfımızı Frontend'e fırlatıyoruz.
                context.Result = new BadRequestObjectResult(response);

                return; // next() metodunu ÇAĞIRMIYORUZ. İstek burada kesildi ve Controller'a gitmesi engellendi.
            }

            // Hata yoksa istek yoluna devam etsin.
            await next();
        }
    }
}