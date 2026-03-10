using Application.DTOs.InventoryItems;

namespace Application.Interfaces.Services
{
    public interface IInventoryItemService
    {
        Task<List<InventoryItemDto>> GetAllItemsAsync();
        Task<InventoryItemDto> GetByIdAsync(int id);

        // Ekstra: Sadece depoda boş duranları getir (Zimmetlenebilir olanlar)
        Task<List<InventoryItemDto>> GetAvailableItemsAsync();

        Task CreateAsync(CreateInventoryItemDto createInventoryItemDto);
        Task UpdateAsync(UpdateInventoryItemDto updateInventoryItemDto);
        Task DeleteAsync(int id);
    }
}