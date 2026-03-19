using Application.DTOs.InventoryItems;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class InventoryItemService(IMapper mapper,IUnitOfWork unitOfWork): IInventoryItemService
    {
        public async Task<List<InventoryItemDto>> GetAllItemsAsync()
        {
           var InventoryItems= await unitOfWork.InventoryItems.GetAllAsync();
            return mapper.Map<List<InventoryItemDto>>(InventoryItems);

        }
        public async  Task<InventoryItemDto> GetByIdAsync(int id) { 
        var InventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(id);
            if (InventoryItem == null)
            {
                throw new Exception("Aranan envanter öğesi bulunamadı.");
            }
            return mapper.Map<InventoryItemDto>(InventoryItem);
        }

        // Ekstra: Sadece depoda boş duranları getir (Zimmetlenebilir olanlar)
      public async  Task<List<InventoryItemDto>> GetAvailableItemsAsync() {
            var availableItems = await unitOfWork.InventoryItems.FindAsync(i => i.Status == ItemStatus.Available);
            return mapper.Map<List<InventoryItemDto>>(availableItems);

        }

        public async Task CreateAsync(CreateInventoryItemDto createInventoryItemDto) {
            var existingItem = await unitOfWork.InventoryItems.GetByItemCodeAsync(createInventoryItemDto.ItemCode);
            if(existingItem != null)
            {
                throw new Exception("Bu Demirbaş koduna sahip bir envanter zaten mevcut.");
            }

            var inventoryItem= mapper.Map<InventoryItem>(createInventoryItemDto);
            await unitOfWork.InventoryItems.AddAsync(inventoryItem);
            await unitOfWork.SaveChangesAsync();
        }
    
        public async Task UpdateAsync(UpdateInventoryItemDto updateInventoryItemDto) {
            var existingItem = await unitOfWork.InventoryItems.GetByIdAsync(updateInventoryItemDto.Id);
            if (existingItem == null)
            {
                throw new Exception("Güncellenmek istenilen envanter bulunamadı.");
            }
            //eğer itemCode değiştirilmeye çalışılıyorsa
            if (existingItem.ItemCode != updateInventoryItemDto.ItemCode)
            { //kullanıcının girdiği yeni itemCode ile aynı koda sahip başka bir envanter var mı kontrol et
                var itemWithSameCode = await unitOfWork.InventoryItems.GetByItemCodeAsync(updateInventoryItemDto.ItemCode);
               //eğer varsa hata fırlat
                if (itemWithSameCode != null)
                {
                    throw new Exception($"'{updateInventoryItemDto.ItemCode}' kodu sistemde başka bir cihaza ait. Lütfen farklı bir kod girin.");
                }
            }
            mapper.Map(updateInventoryItemDto, existingItem);
            unitOfWork.InventoryItems.Update(existingItem);
            await unitOfWork.SaveChangesAsync();
        }
     
        public async Task DeleteAsync(int id) {
            var existingItem = await unitOfWork.InventoryItems.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new Exception("Silinmek istenilen envanter bulunamadı.");
            }
            if(existingItem.Status == ItemStatus.Assigned)
            {
                throw new Exception("Zimmetli bir envanter silinemez. Önce zimmetini kaldırın.");
            }

            existingItem.IsDeleted = true;//soft delete
            await unitOfWork.SaveChangesAsync();

        }
    }
}
