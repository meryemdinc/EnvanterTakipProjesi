using Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    // Bütün API Controller'larımız bu rotayı ve API özelliklerini miras alacak
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        // Bu metot, oluşturduğumuz Response<T> zarfını alır
        // ve içindeki StatusCode neyse (200, 201, 204) ona uygun IActionResult üretir.
        [NonAction] // Swagger'ın bu metodu bir API ucu (endpoint) sanmasını engelliyoruz
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            // Eğer HTTP 204 (No Content) ise geriye data dönülmez, sadece durum kodu dönülür
            if (response.StatusCode == 204)
            {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }

            // Diğer tüm durumlar için (200 OK, 201 Created vb.) veriyi JSON formatında dön
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}