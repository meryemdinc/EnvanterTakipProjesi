using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IInternRepository : IGenericRepository<Intern>
    {
        Task<Intern?> GetInternWithDetailsAsync(int id);
        Task<IEnumerable<Intern>> GetAllInternsWithDetailsAsync();

        Task<IEnumerable<Intern>> GetInternsByUniversityAsync(int universityId);
        Task<IEnumerable<Intern>> GetActiveInternsAsync();// Aktif stajyerleri getirir (end date ,start date,IsDeleted)
    }
}
