namespace Application.DTOs.AppUsers
{
   // Login başarılı olduğunda Frontend'e Token ve kullanıcı bilgilerini döneriz.
   public class AuthResponseDto
    {
        public string Token { get; set; } = null!; // JWT Token
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }
}
