using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        // 1. Departmanı, içindeki çalışan listesiyle birlikte getir
        Task<Department?> GetDepartmentWithEmployeesAsync(int id);

        // 2. Tüm departmanları, çalışanlarıyla birlikte listele
        Task<IEnumerable<Department>> GetAllDepartmentsWithEmployeesAsync();

        // 3. İsme göre departman bul (Validasyon için: Aynı isimde departman var mı?)
        Task<Department?> GetByNameAsync(string name);
    }
}
