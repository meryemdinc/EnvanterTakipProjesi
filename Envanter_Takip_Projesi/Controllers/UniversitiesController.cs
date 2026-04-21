using Application.DTOs.Common;
using Application.DTOs.Universities;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    // Rotası otomatik olarak "api/Universities" olacak
    public class UniversitiesController(IUniversityService universityService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // 1. Servisten yalın veriyi al
            var universities = await universityService.GetAllUniversitiesAsync();

            // 2. Veriyi standart zarfımıza (Response) koy ve 200 OK kodunu ekle
            var response = Response<List<UniversityDto>>.Success(universities, 200);

            // 3. BaseController'daki sihirli metoda gönder
            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var university = await universityService.GetByIdAsync(id);
            var response = Response<UniversityDto>.Success(university, 200);
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUniversityDto createUniversityDto)
        {
            await universityService.CreateAsync(createUniversityDto);

            // Veri oluşturulduğunda NoContent dönüyoruz (Gerçek DTO dönmek istersen 201 Created de yapılabilir)
            // Biz servisimizde void (Task) döndüğümüz için data yollamıyoruz.
            var response = Response<NoContent>.Success(204);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUniversityDto updateUniversityDto)
        {
            await universityService.UpdateAsync(updateUniversityDto);
            var response = Response<NoContent>.Success(204);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await universityService.DeleteAsync(id);
            var response = Response<NoContent>.Success(204);
            return CreateActionResultInstance(response);
        }
    }
}