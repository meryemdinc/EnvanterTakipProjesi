using Application.DTOs.Common;
using Application.DTOs.Interns;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace Envanter_Takip_Projesi.Controllers
{
    public class InternsController(IInternService internService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var interns = await internService.GetAllInternsAsync();
            return CreateActionResultInstance(Response<List<InternDto>>.Success(interns, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var intern = await internService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<InternDto>.Success(intern, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInternDto createInternDto)
        {
            // 1. Stajyeri Veritabanına Kaydet ve Servisten yeni ID'yi al
            var createdInternId = await internService.CreateAsync(createInternDto);

            // 2. HANGFIRE: Görevin çalışacağı zamanı hesapla (Bitiş tarihinden 3 gün önce)
            var reminderDate = createInternDto.EndDate.AddDays(-3);

            // 3. Eğer hesaplanan tarih şu andan ilerideyse alarmı kur
            if (reminderDate > DateTime.UtcNow)
            {
                var fullName = $"{createInternDto.FirstName} {createInternDto.LastName}";

                // Hangfire'a diyoruz ki: "IHRReminderService içindeki bu metodu, reminderDate tarihi geldiğinde çalıştır!"
                BackgroundJob.Schedule<IHRReminderService>(
                    hrService => hrService.SendInternshipEndingReminderAsync(createdInternId, fullName, createInternDto.Email),
                    reminderDate
                );
            }

            // Controller HTTP cevabı döner (204 No Content)
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateInternDto updateInternDto)
        {
            await internService.UpdateAsync(updateInternDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await internService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}