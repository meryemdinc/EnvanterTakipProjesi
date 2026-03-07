using Domain.Enums;

namespace Application.DTOs.InventoryItems
{
    public class UpdateInventoryItemDto
    {
        public int Id { get; set; }
        public string ItemCode { get; set; } = null!;
        public string? SerialNumber { get; set; }
        public string Category { get; set; } = null!;
        public string? Brand { get; set; }
        public string? Model { get; set; }

        public ItemStatus Status { get; set; }

        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string? Notes { get; set; }
    }
}