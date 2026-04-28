using Application.Exceptions;
using Application.Interfaces;

namespace Application.Managers
{
    public class DepartmentManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfNameIsUniqueAsync(string name, int? currentId = null)
        {
            var existingDept = await unitOfWork.Departments.GetByNameAsync(name);
            if (existingDept != null && existingDept.Id != currentId)
            {
                throw new BadRequestException("Bu departman adı zaten kullanımda.");
            }
        }

        public async Task CheckIfHasEmployeesBeforeDeleteAsync(int departmentId)
        {
            var employees = await unitOfWork.Employees.GetEmployeesByDepartmentAsync(departmentId);
            if (employees.Any())
            {
                throw new BadRequestException("Bu departmanda çalışan personeller var, silinemez!");
            }
        }
    }
}