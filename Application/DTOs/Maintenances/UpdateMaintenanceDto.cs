namespace Application.DTOs.Maintenances
{
    public class UpdateMaintenanceDto
    {
        public int Id { get; set; }

        public DateTime? RepairedAt { get; set; } // Tamir bitti mi? Tarihi gir.
        public string? PerformedBy { get; set; }
        public decimal? Cost { get; set; }
        public string? Description { get; set; }
    }
}