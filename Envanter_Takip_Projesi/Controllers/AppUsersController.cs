using Application.DTOs.AppUsers;
using Application.DTOs.Common;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class AppUsersController(IAppUserService appUserService) : CustomBaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Login servisi hata fırlatmazsa (şifre yanlış vb. değilse) AuthResponseDto döner (İçinde JWT Token var)
            var result = await appUserService.LoginAsync(loginDto);
            return CreateActionResultInstance(Response<AuthResponseDto>.Success(result, 200));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await appUserService.RegisterAsync(registerDto);
            return CreateActionResultInstance(Response<AuthResponseDto>.Success(result, 201)); // 201 Created
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await appUserService.GetAllUsersAsync();
            return CreateActionResultInstance(Response<List<AppUserDto>>.Success(users, 200));
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await appUserService.GetUserByEmailAsync(email);
            return CreateActionResultInstance(Response<AppUserDto>.Success(user, 200));
        }

        [HttpPut("assign-role")]
        public async Task<IActionResult> AssignRole(int userId, string role)
        {
            await appUserService.AssignRoleToUserAsync(userId, role);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}