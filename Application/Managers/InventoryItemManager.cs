using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Managers
{
    public class InventoryItemManager(IUnitOfWork unitOfWork)
    {
        public async Task CheckIfItemCodeIsUniqueAsync(string itemCode, int? currentId = null)
        {
            var existingItem = await unitOfWork.InventoryItems.GetByItemCodeAsync(itemCode);
            if (existingItem != null && existingItem.Id != currentId)
            {
                throw new BadRequestException($"'{itemCode}' kodu sistemde zaten kayıtlı. Lütfen farklı bir kod girin.");
            }
        }

        public void CheckIfCanBeDeleted(InventoryItem item)
        {
            if (item.Status == ItemStatus.Assigned)
            {
                throw new BadRequestException("Zimmetli bir envanter silinemez. Önce zimmetini kaldırın.");
            }
        }
    }
}