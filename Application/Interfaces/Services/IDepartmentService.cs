using Application.DTOs.Departments;

namespace Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetByIdAsync(int id);
        Task CreateAsync(CreateDepartmentDto createDepartmentDto);
        Task UpdateAsync(UpdateDepartmentDto updateDepartmentDto);
        Task DeleteAsync(int id);

    }
}
