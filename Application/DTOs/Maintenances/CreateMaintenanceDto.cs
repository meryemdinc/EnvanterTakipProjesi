namespace Application.DTOs.Maintenances
{
    public class CreateMaintenanceDto
    {
        public int InventoryItemId { get; set; } // Hangi cihaz bozuldu?

        public DateTime ReportedAt { get; set; } = DateTime.Now; // Varsayılan: Şu an
        public string? Description { get; set; } // Arıza notu (Örn: Ekran kırık)
    }
}