namespace Application.DTOs.Assignments
{
    public class CreateAssignmentDto
    { 
        public int InventoryItemId { get; set; }
        public int? InternId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }
}
