using Application.DTOs.AppUsers;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using BCrypt.Net;
using IdentityServer4.Services; // BCrypt paketini kurduğunu varsayıyoruz

namespace Application.Services
{
    public class AppUserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService) : IAppUserService
    {
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Kendi yazdığın özel metodu kullanarak Email ile kullanıcıyı getiriyoruz
            var user = await unitOfWork.AppUsers.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            // 2. ŞİFRE DOĞRULAMA (Kullanıcının girdiği düz şifre vs DB'deki Hash)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Hatalı şifre.");
            }

            // 3. Giriş Başarılı! DTO'ya çevir ve Token ekle
            var responseDto = mapper.Map<AuthResponseDto>(user);
            responseDto.Token = tokenService.CreateToken(user);

            return responseDto;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. Bu Email zaten var mı kontrolü (Yine senin özel metodunla)
            var existingUser = await unitOfWork.AppUsers.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Bu e-posta adresi zaten kullanımda.");
            }

            // 2. DTO'yu Entity'e çevir (Mapper şifreyi atlayacak)
            var newUser = mapper.Map<AppUser>(registerDto);

            // 3. ŞİFREYİ HASHLE
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // 4. Generic Repository'deki AddAsync ile veritabanına ekle
            await unitOfWork.AppUsers.AddAsync(newUser);
            await unitOfWork.SaveChangesAsync();

            // 5. Employee Bağlantısı (Eğer dışarıdan bir EmployeeId gönderildiyse)
            if (registerDto.EmployeeId.HasValue)
            {
                var employee = await unitOfWork.Employees.GetByIdAsync(registerDto.EmployeeId.Value);
                if (employee != null)
                {
                    employee.AppUserId = newUser.Id;
                    unitOfWork.Employees.Update(employee);
                    await unitOfWork.SaveChangesAsync();
                }
            }

            // 6. Kayıt başarılı, direkt giriş yapmış gibi token dön
            var responseDto = mapper.Map<AuthResponseDto>(newUser);
            responseDto.Token = tokenService.CreateToken(newUser);

            return responseDto;
        }

        public async Task<List<AppUserDto>> GetAllUsersAsync()
        {
            // DİKKAT: Generic olan GetAllAsync yerine, senin yazdığın detaylı olanı kullandık.
            // Böylece Employee (Çalışan) bilgileri de null gelmeyecek!
            var users = await unitOfWork.AppUsers.GetAllAppUsersWithDetailsAsync();
            return mapper.Map<List<AppUserDto>>(users);
        }

        public async Task<AppUserDto> GetUserByEmailAsync(string email)
        {
            var user = await unitOfWork.AppUsers.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
            return mapper.Map<AppUserDto>(user);
        }

        public async Task AssignRoleToUserAsync(string userId, string role)
        {
            // userId string geldiği için int'e çeviriyoruz
            if (!int.TryParse(userId, out int id))
            {
                throw new Exception("Geçersiz ID formatı.");
            }

            var user = await unitOfWork.AppUsers.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            // Rolü güncelle ve Generic Repository'nin Update metodunu çağır
            user.Role = role;
            unitOfWork.AppUsers.Update(user);
            await unitOfWork.SaveChangesAsync();
        }

        public Task CreateRoleAsync(string roleName)
        {
            throw new NotImplementedException("Role tablosu olmadığı için bu metot kullanılamaz. Rolleri sabit (Constant) olarak yönetmelisiniz.");
        }
    }
}