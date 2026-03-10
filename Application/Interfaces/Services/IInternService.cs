using Application.DTOs.Interns;

namespace Application.Interfaces.Services
{
    public interface IInternService
    {
        Task<List<InternDto>> GetAllInternsAsync();
        Task<InternDto> GetByIdAsync(int id);
        Task CreateAsync(CreateInternDto createInternDto);
        Task UpdateAsync(UpdateInternDto updateInternDto);
        Task DeleteAsync(int id);
    }
}