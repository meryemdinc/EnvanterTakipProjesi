using Application.DTOs.Departments;
using Application.Interfaces.Services;
using AutoMapper;
using Application.Interfaces;
using Domain.Entities;
namespace Application.Services
{
    public class DepartmentService(IMapper mapper,IUnitOfWork unitOfWork):IDepartmentService
    {
       public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await unitOfWork.Departments.GetAllAsync();
           return mapper.Map<List<DepartmentDto>>(departments);
        }
       public async Task<DepartmentDto> GetByIdAsync(int id)
        {
            var existingDepartment = await unitOfWork.Departments.GetByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new Exception("Department not found");
            }
           return mapper.Map<DepartmentDto>(existingDepartment);
        }
        public async Task CreateAsync(CreateDepartmentDto createDepartmentDto)
        {
            var existingDepartment = await unitOfWork.Departments.GetByNameAsync(createDepartmentDto.Name);
            if(existingDepartment != null)
            {
                throw new Exception("Department already exists");
            }
            var department = mapper.Map<Department>(createDepartmentDto);
            await unitOfWork.Departments.AddAsync(department);
            await unitOfWork.SaveChangesAsync();
        }
      public async Task UpdateAsync(UpdateDepartmentDto updateDepartmentDto)
        {
            var department= await unitOfWork.Departments.GetByIdAsync(updateDepartmentDto.Id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
          
            var existingNameDept = await unitOfWork.Departments.GetByNameAsync(updateDepartmentDto.Name);

            if (existingNameDept != null && existingNameDept.Id != department.Id)
            {
                throw new Exception("Bu departman adı zaten başka bir departman tarafından kullanılıyor.");
            }
            mapper.Map(updateDepartmentDto, department);
           unitOfWork.Departments.Update(department);
           await unitOfWork.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
           var employees = await unitOfWork.Employees.GetEmployeesByDepartmentAsync(department.Id);
            if(employees.Any())
            {
                throw new Exception("Department has employees, cannot be deleted");
            }
            unitOfWork.Departments.Delete(department);
             await unitOfWork.SaveChangesAsync();
        }
    }
}
