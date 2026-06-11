using Application.DTOs.Assignments;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers;
using Application.Messages; // RabbitMQ Mesaj şablonumuz için eklendi
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MassTransit; // IPublishEndpoint için eklendi

namespace Application.Services
{
    // DİKKAT: IPublishEndpoint publishEndpoint parametresi eklendi!
    public class AssignmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        AssignmentManager assignmentManager,
        IPublishEndpoint publishEndpoint) : IAssignmentService
    {
        public async Task<List<AssignmentDto>> GetAllAssignmentsAsync()
        {
            var assignments = await unitOfWork.Assignments.GetAllAsync();
            return mapper.Map<List<AssignmentDto>>(assignments);
        }

        public async Task<AssignmentDto> GetByIdAsync(int id)
        {
            var assignment = await unitOfWork.Assignments.GetByIdAsync(id);
            if (assignment == null)
            {
                throw new NotFoundException($"{id} ID'li zimmet bulunamadı.");
            }
            return mapper.Map<AssignmentDto>(assignment);
        }

        public async Task CreateAsync(CreateAssignmentDto createAssignmentDto)
        {
            // 1. Eşyayı bul
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(createAssignmentDto.InventoryItemId);
            if (inventoryItem == null)
            {
                throw new NotFoundException("Zimmetlenmek istenen eşya bulunamadı.");
            }

            // 2. KURALLARI ÇALIŞTIR (Hata varsa fırlatır, kod aşağıya inmez)
            assignmentManager.CheckIfItemIsAvailableForAssignment(inventoryItem);
            await assignmentManager.CheckIfUserAlreadyHasSameCategoryItemAsync(createAssignmentDto.EmployeeId, inventoryItem.Category);

            // 3. Kurallardan geçtiyse zimmet işlemini yap
            var assignment = mapper.Map<Assignment>(createAssignmentDto);
            await unitOfWork.Assignments.AddAsync(assignment);

            // 4. Eşya durumunu güncelle
            inventoryItem.Status = ItemStatus.Assigned;
            unitOfWork.InventoryItems.Update(inventoryItem);

            // 5. Kaydet (Veritabanı transaction işlemi burada biter)
            await unitOfWork.SaveChangesAsync();

            // 6. 🚀 RABBITMQ'YA MESAJ FIRLAT
            // Name özelliği olmadığı için Brand, Model ve ItemCode'u birleştirerek anlamlı bir isim oluşturuyoruz.
            string deviceName = $"{inventoryItem.Brand} {inventoryItem.Model} ({inventoryItem.ItemCode})".Trim();

            await publishEndpoint.Publish(new InventoryAssignedEvent
            {
                EmployeeFullName = "Meryem Dinç",
                EmployeeEmail = "meryem@esogu.edu.tr",
                ItemName = deviceName, // Birleştirdiğimiz metni buraya veriyoruz
                AssignedAt = DateTime.UtcNow
            });
        }

        public async Task UpdateAsync(UpdateAssignmentDto updateAssignmentDto)
        {
            var existingAssignment = await unitOfWork.Assignments.GetByIdAsync(updateAssignmentDto.Id);
            if (existingAssignment == null)
            {
                throw new NotFoundException("Güncellenecek zimmet kaydı bulunamadı.");
            }

            mapper.Map(updateAssignmentDto, existingAssignment);
            unitOfWork.Assignments.Update(existingAssignment);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ReturnItemAsync(ReturnAssignmentDto returnAssignmentDto)
        {
            var existingAssignment = await unitOfWork.Assignments.GetByIdAsync(returnAssignmentDto.Id);
            if (existingAssignment == null)
            {
                throw new NotFoundException("Güncellenecek zimmet kaydı bulunamadı.");
            }

            // 1. KURALI ÇALIŞTIR: Zaten iade edilmiş mi?
            assignmentManager.CheckIfAlreadyReturned(existingAssignment);

            // 2. İade işlemini gerçekleştir
            mapper.Map(returnAssignmentDto, existingAssignment);
            unitOfWork.Assignments.Update(existingAssignment);

            // 3. Eşyayı depoya (Available) geri al
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(existingAssignment.InventoryItemId);
            if (inventoryItem != null)
            {
                inventoryItem.Status = ItemStatus.Available;
                unitOfWork.InventoryItems.Update(inventoryItem);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingAssignment = await unitOfWork.Assignments.GetByIdAsync(id);
            if (existingAssignment == null)
            {
                throw new NotFoundException("Silinecek zimmet kaydı bulunamadı.");
            }

            unitOfWork.Assignments.Delete(existingAssignment);
            await unitOfWork.SaveChangesAsync();
        }
    }
}