using Application.DTOs.Employees;

namespace Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetByIdAsync(int id);
        Task CreateAsync(CreateEmployeeDto createEmployeeDto);
        Task UpdateAsync(UpdateEmployeeDto updateEmployeeDto);
        Task DeleteAsync(int id);
    }
}