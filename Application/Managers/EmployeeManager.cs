using Application.Exceptions;
using Application.Interfaces;

namespace Application.Managers
{
    public class EmployeeManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfEmailIsUniqueAsync(string email, int? currentId = null)
        {
            var existingEmployee = await unitOfWork.Employees.GetByEmailAsync(email);
            if (existingEmployee != null && existingEmployee.Id != currentId)
            {
                throw new BadRequestException("Bu e-posta adresi zaten başka bir personele ait.");
            }
        }

        public async Task CheckIfDepartmentExistsAsync(int departmentId)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(departmentId);
            if (department == null)
            {
                throw new BadRequestException("Belirtilen departman bulunamadı. Lütfen geçerli bir departman seçin.");
            }
        }
    }
}