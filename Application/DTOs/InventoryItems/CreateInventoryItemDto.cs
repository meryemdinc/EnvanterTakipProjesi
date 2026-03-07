using Domain.Enums;

namespace Application.DTOs.InventoryItems
{
    public class CreateInventoryItemDto
    {
        public string ItemCode { get; set; } = null!;
        public string? SerialNumber { get; set; }
        public string Category { get; set; } = null!; // "Laptop", "Monitor"
        public string? Brand { get; set; }
        public string? Model { get; set; }

        public ItemStatus Status { get; set; } = ItemStatus.Available; // Varsayılan: Boşta

        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; } // Garanti Bitiş Tarihi
        public string? Notes { get; set; }
    }
}
