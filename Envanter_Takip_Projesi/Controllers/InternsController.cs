using Application.DTOs.Common;
using Application.DTOs.Interns;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
            await internService.CreateAsync(createInternDto);
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