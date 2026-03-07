namespace Application.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // İlişkiler (Flattening)
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; } // Örn: "Yazılım Ekibi"

        // Kullanıcı hesabı var mı? (AppUserId doluysa true dönecek şekilde mapleyeceğiz)
        public bool HasSystemAccess { get; set; }
    }
}