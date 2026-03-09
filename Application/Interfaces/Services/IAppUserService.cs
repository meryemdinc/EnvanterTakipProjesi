using Application.DTOs.AppUsers;


namespace Application.Interfaces.Services
{
    public interface IAppUserService
    {
        // ==========================
        // 1. AUTH (Giriş & Kayıt)
        // ==========================

        // Kullanıcı giriş yapar, geriye Token ve bilgiler döner
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        // Kullanıcı kayıt olur, başarılıysa Token döner (Auto-Login)
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        // Şifremi unuttum / Şifre değiştir (İleride eklenebilir)
        // Task ForgotPasswordAsync(string email);


        // ==========================
        // 2. USER MANAGEMENT (Kullanıcı Yönetimi)
        // ==========================

        // Tüm kullanıcıları listele (Admin paneli için)
        Task<List<AppUserDto>> GetAllUsersAsync();

        // Email ile tek bir kullanıcı getir
        Task<AppUserDto> GetUserByEmailAsync(string email);

        // Kullanıcıya rol atama (Örn: "Admin" yapma)
        Task AssignRoleToUserAsync(string userId, string role);

        // Sisteme yeni rol tanımlama (Örn: "Manager" rolü oluştur)
        Task CreateRoleAsync(string roleName);
    }
}