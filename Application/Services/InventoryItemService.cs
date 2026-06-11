using Application.DTOs.InventoryItems;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers; // Manager'ı kullanmak için ekledik
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class InventoryItemService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        InventoryItemManager inventoryItemManager) : IInventoryItemService // Manager Constructor'a eklendi
    {
        public async Task<List<InventoryItemDto>> GetAllItemsAsync()
        {
            // Değişken ismini standartlara uygun olarak küçük harfle başlattık
            var inventoryItems = await unitOfWork.InventoryItems.GetAllAsync();
            return mapper.Map<List<InventoryItemDto>>(inventoryItems);
        }

        public async Task<InventoryItemDto> GetByIdAsync(int id)
        {
            var inventoryItem = await unitOfWork.InventoryItems.GetByIdAsync(id);
            if (inventoryItem == null)
            {
                throw new NotFoundException("Aranan envanter öğesi bulunamadı.");
            }
            return mapper.Map<InventoryItemDto>(inventoryItem);
        }

        // Ekstra: Sadece depoda boş duranları getir (Zimmetlenebilir olanlar)
        public async Task<List<InventoryItemDto>> GetAvailableItemsAsync()
        {
            var availableItems = await unitOfWork.InventoryItems.FindAsync(i => i.Status == ItemStatus.Available);
            return mapper.Map<List<InventoryItemDto>>(availableItems);
        }

        public async Task CreateAsync(CreateInventoryItemDto createInventoryItemDto)
        {
            // 1. KURALI ÇALIŞTIR: Demirbaş kodu benzersiz mi?
            await inventoryItemManager.CheckIfItemCodeIsUniqueAsync(createInventoryItemDto.ItemCode);

            // 2. KAYDET
            var inventoryItem = mapper.Map<InventoryItem>(createInventoryItemDto);
            await unitOfWork.InventoryItems.AddAsync(inventoryItem);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateInventoryItemDto updateInventoryItemDto)
        {
            var existingItem = await unitOfWork.InventoryItems.GetByIdAsync(updateInventoryItemDto.Id);
            if (existingItem == null)
            {
                throw new NotFoundException("Güncellenmek istenilen envanter bulunamadı.");
            }

            // 1. KURALI ÇALIŞTIR: Yeni girilen kod başkasına ait mi? (Kendi ID'sini gönderiyoruz)
            await inventoryItemManager.CheckIfItemCodeIsUniqueAsync(updateInventoryItemDto.ItemCode, existingItem.Id);

            // 2. GÜNCELLE
            mapper.Map(updateInventoryItemDto, existingItem);
            unitOfWork.InventoryItems.Update(existingItem);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingItem = await unitOfWork.InventoryItems.GetByIdAsync(id);
            if (existingItem == null)
            {
                throw new NotFoundException("Silinmek istenilen envanter bulunamadı.");
            }

            // 1. KURALI ÇALIŞTIR: Zimmetli eşya silinemez kuralını denetle
            inventoryItemManager.CheckIfCanBeDeleted(existingItem);

            // 2. SİL (Soft Delete)
            existingItem.IsDeleted = true;
            unitOfWork.InventoryItems.Update(existingItem);
            await unitOfWork.SaveChangesAsync();
        }
    }
}