namespace Application.DTOs.Assignments
{
    public class ReturnAssignmentDto
    {
        public int Id { get; set; } // Zimmet ID'si
        public DateTime ActualReturnAt { get; set; } = DateTime.Now; // İade tarihi
        public string? Notes { get; set; } // Örn: "Ekranı çizik teslim etti."
    }
}