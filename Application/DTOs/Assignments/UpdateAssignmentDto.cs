namespace Application.DTOs.Assignments
{
    public class UpdateAssignmentDto
    {
        public int Id { get; set; } // Hangi kayıt?

        // Dikkat: Eşyanın kendisini (InventoryItemId) değiştirmek genelde risklidir 
        // ama yanlış girildiyse değiştirilmesine izin verelim.
        public int InventoryItemId { get; set; }

        // Yanlış kişiye zimmetlendiyse düzeltmek için:
        public int? InternId { get; set; }
        public int? EmployeeId { get; set; }

        public DateTime AssignedAt { get; set; } // Tarih hatasını düzeltmek için
        public DateTime? ActualReturnAt { get; set; } // İade tarihini elle düzeltmek için
        public string? Notes { get; set; }
    }
}