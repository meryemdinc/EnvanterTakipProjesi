namespace Application.DTOs.InventoryItems
{
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public string ItemCode { get; set; } = null!; // Demirbaş No
        public string? SerialNumber { get; set; }
        public string Category { get; set; } = null!;
        public string? Brand { get; set; }
        public string? Model { get; set; }

        // Enum'ı Frontend'e string olarak ("Available", "InUse" vb.) göndermek daha okunaklıdır.
        public string Status { get; set; } = null!;

        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string? Notes { get; set; }

        // MAPPING İLE DOLACAK KRİTİK ALANLAR:
        // Bu eşya şu an kimde? (Ahmet Yılmaz veya Stajyer Mehmet)
        public string? CurrentHolderName { get; set; }
    }
}