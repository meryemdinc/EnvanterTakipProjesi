using Application.DTOs.Universities;

namespace Application.Interfaces.Services
{
    public interface IUniversityService
    {
        Task<List<UniversityDto>> GetAllUniversitiesAsync();
        Task<UniversityDto> GetByIdAsync(int id);
        Task CreateAsync(CreateUniversityDto createUniversityDto);
        Task UpdateAsync(UpdateUniversityDto updateUniversityDto);
        Task DeleteAsync(int id);
    }
}
