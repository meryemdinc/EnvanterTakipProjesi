using Application.Interfaces.Services;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs.Interns;
using Domain.Entities;
using Domain.Enums;
namespace Application.Services
{
    public class InternService(IMapper mapper,IUnitOfWork unitOfWork):IInternService
    {
       public async Task<List<InternDto>> GetAllInternsAsync() { 
        var interns = await unitOfWork.Interns.GetAllAsync();   
            return mapper.Map<List<InternDto>>(interns);
        }
      public async Task<InternDto> GetByIdAsync(int id) { 
        var intern=await unitOfWork.Interns.GetByIdAsync(id);
            if (intern == null)
            {
                throw new Exception("Aranan stajyer bulunamadı.");
            }
            return mapper.Map<InternDto>(intern);
        }
     public async Task CreateAsync(CreateInternDto createInternDto) {
        var existingIntern = await unitOfWork.Interns.GetByEmailAsync(createInternDto.Email);
            if (existingIntern != null)
            {
                throw new Exception("Bu e-posta adresli stajyer zaten ekli.");
            }
            var intern = mapper.Map<Intern>(createInternDto);
            await unitOfWork.Interns.AddAsync(intern);
            await unitOfWork.SaveChangesAsync();
        }
       public async Task UpdateAsync(UpdateInternDto updateInternDto) { 
        var existingIntern=await unitOfWork.Interns.GetByIdAsync(updateInternDto.Id);
            if (existingIntern == null)
            {
                throw new Exception("Güncellenecek stajyer bulunamadı.");
            }
            mapper.Map(updateInternDto, existingIntern);
            unitOfWork.Interns.Update(existingIntern);
            await unitOfWork.SaveChangesAsync();

        }
       public async Task DeleteAsync(int id) { 
        var existingIntern= await unitOfWork.Interns.GetByIdAsync(id);
            if (existingIntern == null)
            {
                throw new Exception("Silinecek stajyer bulunamadı.");
            }

            var activeAssignments = await unitOfWork.Assignments.GetActiveAssignmentsByInternIdAsync(id);
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
            existingIntern.IsDeleted = true; // Soft delete
            unitOfWork.Interns.Update(existingIntern);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
