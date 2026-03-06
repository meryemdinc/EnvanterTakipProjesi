namespace Application.DTOs.Assignments
{
    public class AssignmentDto
    {
        public int Id { get; set; }

        public int InventoryItemId { get; set; }
        public string InventoryItemName { get; set; } = null!; // Örn: "Dell XPS 15"
        public string SerialNumber { get; set; } // Eşyanın seri nosu da görünsün

        // Zimmet kime yapıldı? (İkisinden biri dolu olacak)
        public int? InternId { get; set; }
        public string? InternName { get; set; }

        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        // Kolaylık olsun diye tek bir alanda gösterim (Frontend için)
        // Örn: "Ahmet Yılmaz (Stajyer)"
        public string AssignedTo { get; set; } = null!;

        public DateTime AssignedAt { get; set; }
        public DateTime? ActualReturnAt { get; set; } // Doluysa iade edilmiştir
        public string? Notes { get; set; }
    }
}