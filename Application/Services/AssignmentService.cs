using Application.DTOs.Assignments;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

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
        public async Task<AssignmentDto> GetByIdAsync(int id)
        {
            var assignments = await unitOfWork.Assignments.GetByIdAsync(id);
            return mapper.Map<AssignmentDto>(assignments);
        }

        // Yeni zimmet oluşturma
        // Yeni zimmet oluşturma
        public async Task CreateAsync(CreateAssignmentDto createAssignmentDto)
        {
            // 1. Önce zimmetlenecek eşyayı bulalım
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(createAssignmentDto.InventoryItemId);

            if (inventoryItem == null)
            {
                throw new Exception("Zimmetlenmek istenen eşya bulunamadı.");
            }

            // 2. Bu eşya şu an boşta mı? (Sadece Available olanlar zimmetlenebilir)
            if (inventoryItem.Status != ItemStatus.Available)
            {
                throw new Exception($"Bu eşya şu anda zimmetlenemez. Mevcut durumu: {inventoryItem.Status}");
            }

            // 3. Eşya boşta! O zaman zimmet işlemini yap
            var assignment = mapper.Map<Assignment>(createAssignmentDto);
            await unitOfWork.Assignments.AddAsync(assignment);

            // 4. Eşyanın durumunu "Kullanımda" olarak güncelle! (Kendi Enum'ına göre düzenle, örn: InUse)
            inventoryItem.Status = ItemStatus.Assigned;
            unitOfWork.InventoryItems.Update(inventoryItem);

            // 5. Her ikisini de (Zimmet kaydı ve Eşya durum güncellemesi) aynı anda kaydet
            await unitOfWork.SaveChangesAsync();
        }

        // Hatalı zimmet düzeltme
        public async Task UpdateAsync(UpdateAssignmentDto updateAssignmentDto)
        {
            var existingAssignment = await unitOfWork.Assignments.GetByIdAsync(updateAssignmentDto.Id);
            if (existingAssignment == null)
            {
                throw new Exception("Güncellenecek zimmet kaydı bulunamadı.");
            }
            //Map(kaynak,hedef)-> existingAssignment'a updateAssignmentDto ı kopyalar,üzerine yapıştırır.
            mapper.Map(updateAssignmentDto, existingAssignment);
            unitOfWork.Assignments.Update(existingAssignment);
            await unitOfWork.SaveChangesAsync();
        }

        // ÖZEL: Zimmeti iade alma (Stajyer bilgisayarı geri getirdi)
        public async Task ReturnItemAsync(ReturnAssignmentDto returnAssignmentDto)
        {
            var existingAssignment = await unitOfWork.Assignments.GetByIdAsync(returnAssignmentDto.Id);
            if (existingAssignment == null)
            {
                throw new Exception("Güncellenecek zimmet kaydı bulunamadı.");
            }
            mapper.Map(returnAssignmentDto, existingAssignment);
            unitOfWork.Assignments.Update(existingAssignment);
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(existingAssignment.InventoryItemId);

            if (inventoryItem != null)
            {
                inventoryItem.Status= ItemStatus.Available; // Eşyayı tekrar kullanılabilir yapıyoruz

                unitOfWork.InventoryItems.Update(inventoryItem);
            }
            await unitOfWork.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var existingAssignment =await unitOfWork.Assignments.GetByIdAsync(id);
            if (existingAssignment == null)
            {
                throw new Exception("Silinecek zimmet kaydı bulunamadı.");
            }
            unitOfWork.Assignments.Delete(existingAssignment);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
