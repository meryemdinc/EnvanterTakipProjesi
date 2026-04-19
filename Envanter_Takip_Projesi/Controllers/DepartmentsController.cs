using Application.DTOs.Common;
using Application.DTOs.Departments;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class DepartmentsController(IDepartmentService departmentService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await departmentService.GetAllDepartmentsAsync();
            return CreateActionResultInstance(Response<List<DepartmentDto>>.Success(departments, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await departmentService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<DepartmentDto>.Success(department, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto createDepartmentDto)
        {
            await departmentService.CreateAsync(createDepartmentDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDepartmentDto updateDepartmentDto)
        {
            await departmentService.UpdateAsync(updateDepartmentDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await departmentService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}