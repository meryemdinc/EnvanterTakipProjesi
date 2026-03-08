namespace Application.DTOs.Maintenances
{
    public class MaintenanceDto
    {
        public int Id { get; set; }

        public int InventoryItemId { get; set; }
        // Hangi cihaz? (Örn: "Dell XPS 13 - LPT005")
        public string InventoryItemName { get; set; } = null!;

        public DateTime ReportedAt { get; set; } // Bildirilme Tarihi
        public DateTime? RepairedAt { get; set; } // Tamir Tarihi

        // Frontend'de "Tamamlandı" tiki göstermek için helper bir property
        public bool IsCompleted { get; set; }

        public string? Description { get; set; }
        public string? PerformedBy { get; set; } // Tamirci Firma/Kişi
        public decimal? Cost { get; set; } // Maliyet
    }
}