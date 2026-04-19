using Application.DTOs.Common;
using Application.DTOs.Employees;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class EmployeesController(IEmployeeService employeeService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            return CreateActionResultInstance(Response<List<EmployeeDto>>.Success(employees, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<EmployeeDto>.Success(employee, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto createEmployeeDto)
        {
            await employeeService.CreateAsync(createEmployeeDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmployeeDto updateEmployeeDto)
        {
            await employeeService.UpdateAsync(updateEmployeeDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await employeeService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}