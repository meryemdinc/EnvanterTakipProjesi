using Application.DTOs.AppUsers;


namespace Application.Interfaces.Services
{
    public interface IAppUserService
    {
    

        // Kullanıcı giriş yapar, geriye Token ve bilgiler döner
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        // Kullanıcı kayıt olur, başarılıysa Token döner (Auto-Login)
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        // Tüm kullanıcıları listele (Admin paneli için)
        Task<List<AppUserDto>> GetAllUsersAsync();

        // Email ile tek bir kullanıcı getir
        Task<AppUserDto> GetUserByEmailAsync(string email);

        // Kullanıcıya rol atama (Örn: "Admin" yapma)
        Task AssignRoleToUserAsync(int userId, string role);

    }
}