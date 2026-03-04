namespace Application.DTOs.AppUsers
{
    //kullanıcıyı kaydederken
   public class RegisterDto
    { 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        // Opsiyonel: Bu hesap halihazırda var olan bir çalışana aitse bu kısım doldurulur
        public int? EmployeeId { get; set; }
    }
}
