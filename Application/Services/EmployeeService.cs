using AutoMapper;
using Application.Interfaces;
using Application.DTOs.Employees;
using Domain.Entities;
using Application.Interfaces.Services;
using Domain.Enums;

namespace Application.Services
{
    public class EmployeeService(IMapper mapper,IUnitOfWork unitOfWork):IEmployeeService
    {
       public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await unitOfWork.Employees.GetAllAsync();
            return mapper.Map<List<EmployeeDto>>(employees);
        }
       public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var employee = await unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                throw new Exception("Aranan kişi bulunamadı.");
            }
            return mapper.Map<EmployeeDto>(employee);
        }
       public async Task CreateAsync(CreateEmployeeDto createEmployeeDto)
        {
            var existingEmployee = await unitOfWork.Employees.GetByEmailAsync(createEmployeeDto.Email);
            if (existingEmployee != null)
            {
                throw new Exception("Bu e-posta adresli kişi zaten ekli.");
            }
         var employee= mapper.Map<Employee>(createEmployeeDto);
          await unitOfWork.Employees.AddAsync(employee);
          await unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateAsync(UpdateEmployeeDto updateEmployeeDto)
        {
            var existingEmployee = await unitOfWork.Employees.GetByIdAsync(updateEmployeeDto.Id);
            if (existingEmployee == null)
            {
                throw new Exception("Güncellenecek kişi bulunamadı.");
            }
            mapper.Map(updateEmployeeDto, existingEmployee);
            unitOfWork.Employees.Update(existingEmployee);
           await unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var existingEmployee = await unitOfWork.Employees.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                throw new Exception("Silinecek kişi bulunamadı.");
            }
       
            if (existingEmployee.AppUserId.HasValue)
            {
                var appUser = await unitOfWork.AppUsers.GetByIdAsync(existingEmployee.AppUserId.Value);
                if (appUser != null)
                {
                    appUser.IsDeleted = true; // AppUser'ı da soft delete yap
                    unitOfWork.AppUsers.Update(appUser);
                }
            }
            var activeAssignments = await unitOfWork.Assignments.GetActiveAssignmentsByEmployeeIdAsync(id);
            foreach(var assignment in activeAssignments)
            {
                assignment.ActualReturnAt = DateTime.Now;
                unitOfWork.Assignments.Update(assignment);
                if (assignment.InventoryItem != null)
                {
                    assignment.InventoryItem.Status = ItemStatus.Available;
                    unitOfWork.InventoryItems.Update(assignment.InventoryItem);
                }
            }
            existingEmployee.IsDeleted = true;
            unitOfWork.Employees.Update(existingEmployee);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
