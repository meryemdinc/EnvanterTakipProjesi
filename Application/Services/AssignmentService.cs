using Application.DTOs.Assignments;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;

namespace Application.Services
{
    public class AssignmentService(IUnitOfWork unitOfWork, IMapper mapper) :IAssignmentService
    {
        public async Task<List<AssignmentDto>> GetAllAssignmentsAsync()
        {
            
            var assignments = await unitOfWork.Assignments.GetAllAsync();

            // Elimizdeki "assignments" değişkenini (Entity listesi), DTO listesine çevirdik
            return mapper.Map<List<AssignmentDto>>(assignments);
        }
        public Task<AssignmentDto> GetByIdAsync(int id)
        {
            
        }

        // Yeni zimmet oluşturma
       public Task CreateAsync(CreateAssignmentDto createAssignmentDto)
        {
            return
        }

        // Hatalı zimmet düzeltme
       public Task UpdateAsync(UpdateAssignmentDto updateAssignmentDto)
        {
            return
        }

        // ÖZEL: Zimmeti iade alma (Stajyer bilgisayarı geri getirdi)
       public Task ReturnItemAsync(ReturnAssignmentDto returnAssignmentDto)
        {

        }

       public Task DeleteAsync(int id)
        {

        }
    }
}
