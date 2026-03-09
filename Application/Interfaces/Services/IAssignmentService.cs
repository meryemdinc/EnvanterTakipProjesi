using Application.DTOs.Assignments;

namespace Application.Interfaces.Services
{
    public interface IAssignmentService
    {
        Task<List<AssignmentDto>> GetAllAssignmentsAsync();
        Task<AssignmentDto> GetByIdAsync(int id);

        // Yeni zimmet oluşturma
        Task CreateAsync(CreateAssignmentDto createAssignmentDto);

        // Hatalı zimmet düzeltme
        Task UpdateAsync(UpdateAssignmentDto updateAssignmentDto);

        // ÖZEL: Zimmeti iade alma (Stajyer bilgisayarı geri getirdi)
        Task ReturnItemAsync(ReturnAssignmentDto returnAssignmentDto);

        Task DeleteAsync(int id); // Yanlışlıkla girilen kaydı silme
    }
}