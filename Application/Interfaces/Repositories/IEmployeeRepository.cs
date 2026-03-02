using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // 1. Tek bir çalışanı tüm detaylarıyla (Departman, Zimmetler, Kullanıcı Hesabı) getir
        Task<Employee?> GetEmployeeWithDetailsAsync(int id);

        // 2. Tüm çalışanları listele (Genel liste)
        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();

        // 3. Belirli bir departmandaki çalışanları getir
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);

        // 4. Email'e göre bul (Validasyon ve benzersizlik kontrolü için)
        Task<Employee?> GetByEmailAsync(string email);

        // 5. Şirkette şu an HALA ÇALIŞAN (İşten ayrılmamış) kişileri getir
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    }
}