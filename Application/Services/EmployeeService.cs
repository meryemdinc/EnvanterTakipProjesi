using Application.DTOs.Employees;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers; // Manager eklendi
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class EmployeeService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        EmployeeManager employeeManager) : IEmployeeService // Manager Constructor'a eklendi
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
                throw new NotFoundException("Aranan kişi bulunamadı.");
            }
            return mapper.Map<EmployeeDto>(employee);
        }

        public async Task CreateAsync(CreateEmployeeDto createEmployeeDto)
        {
            // 1. KURALI ÇALIŞTIR: E-posta benzersiz mi?
            await employeeManager.CheckIfEmailIsUniqueAsync(createEmployeeDto.Email);

            // 2. KAYDET
            var employee = mapper.Map<Employee>(createEmployeeDto);
            await unitOfWork.Employees.AddAsync(employee);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateEmployeeDto updateEmployeeDto)
        {
            var existingEmployee = await unitOfWork.Employees.GetByIdAsync(updateEmployeeDto.Id);
            if (existingEmployee == null)
            {
                throw new NotFoundException("Güncellenecek kişi bulunamadı.");
            }

            // 1. KURALI ÇALIŞTIR: Yeni e-posta adresi başkasına ait mi? (Kendi ID'sini gönderiyoruz)
            await employeeManager.CheckIfEmailIsUniqueAsync(updateEmployeeDto.Email, existingEmployee.Id);

            // 2. GÜNCELLE
            mapper.Map(updateEmployeeDto, existingEmployee);
            unitOfWork.Employees.Update(existingEmployee);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingEmployee = await unitOfWork.Employees.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                throw new NotFoundException("Silinecek kişi bulunamadı.");
            }

            // İŞ AKIŞI 1: Bağlı kullanıcı (AppUser) hesabını soft delete yap
            if (existingEmployee.AppUserId.HasValue)
            {
                var appUser = await unitOfWork.AppUsers.GetByIdAsync(existingEmployee.AppUserId.Value);
                if (appUser != null)
                {
                    appUser.IsDeleted = true;
                    unitOfWork.AppUsers.Update(appUser);
                }
            }

            // İŞ AKIŞI 2: Üzerindeki zimmetleri otomatik olarak iade al ve eşyaları depoya (Available) çek
            var activeAssignments = await unitOfWork.Assignments.GetActiveAssignmentsByEmployeeIdAsync(id);
            foreach (var assignment in activeAssignments)
            {
                assignment.ActualReturnAt = DateTime.Now;
                unitOfWork.Assignments.Update(assignment);

                if (assignment.InventoryItem != null)
                {
                    assignment.InventoryItem.Status = ItemStatus.Available;
                    unitOfWork.InventoryItems.Update(assignment.InventoryItem);
                }
            }

            // İŞ AKIŞI 3: Personeli soft delete yap
            existingEmployee.IsDeleted = true;
            unitOfWork.Employees.Update(existingEmployee);

            // Tüm değişiklikleri tek seferde veritabanına yansıt
            await unitOfWork.SaveChangesAsync();
        }
    }
}