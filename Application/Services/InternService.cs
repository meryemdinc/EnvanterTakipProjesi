using Application.DTOs.Interns;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers; // Manager'ı kullanmak için ekledik
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class InternService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        InternManager internManager) : IInternService // Manager Constructor'a eklendi
    {
        public async Task<List<InternDto>> GetAllInternsAsync()
        {
            var interns = await unitOfWork.Interns.GetAllAsync();
            return mapper.Map<List<InternDto>>(interns);
        }

        public async Task<InternDto> GetByIdAsync(int id)
        {
            var intern = await unitOfWork.Interns.GetByIdAsync(id);
            if (intern == null)
            {
                throw new NotFoundException("Aranan stajyer bulunamadı.");
            }
            return mapper.Map<InternDto>(intern);
        }

        public async Task<int> CreateAsync(CreateInternDto createInternDto)
        {
            // 1. KURALI ÇALIŞTIR: E-posta benzersiz mi?
            await internManager.CheckIfEmailIsUniqueAsync(createInternDto.Email);

            // 2. KAYDET
            var intern = mapper.Map<Intern>(createInternDto);
            intern.StartDate = intern.StartDate.ToUniversalTime();
            intern.EndDate = intern.EndDate.ToUniversalTime();
            await unitOfWork.Interns.AddAsync(intern);
            await unitOfWork.SaveChangesAsync();
            return intern.Id;
        }

        public async Task UpdateAsync(UpdateInternDto updateInternDto)
        {
            var existingIntern = await unitOfWork.Interns.GetByIdAsync(updateInternDto.Id);
            if (existingIntern == null)
            {
                throw new NotFoundException("Güncellenecek stajyer bulunamadı.");
            }

            // 1. KURALI ÇALIŞTIR: Yeni e-posta adresi başkasına ait mi? (Kendi ID'sini gönderiyoruz)
            await internManager.CheckIfEmailIsUniqueAsync(updateInternDto.Email, existingIntern.Id);

            // 2. GÜNCELLE
            mapper.Map(updateInternDto, existingIntern);
            unitOfWork.Interns.Update(existingIntern);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingIntern = await unitOfWork.Interns.GetByIdAsync(id);
            if (existingIntern == null)
            {
                throw new NotFoundException("Silinecek stajyer bulunamadı.");
            }

            // İŞ AKIŞI: Üzerindeki zimmetleri otomatik olarak iade al ve eşyaları depoya (Available) çek
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

            // Stajyeri soft delete yap
            existingIntern.IsDeleted = true;
            unitOfWork.Interns.Update(existingIntern);
            await unitOfWork.SaveChangesAsync();
        }
    }
}