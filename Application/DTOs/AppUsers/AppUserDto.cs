namespace Application.DTOs.AppUsers
{  //admin panelinde kullanıcıları listelerken kullanılır.
    public class AppUserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; } 

    }
}
