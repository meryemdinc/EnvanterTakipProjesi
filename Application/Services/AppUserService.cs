using Application.DTOs.AppUsers;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Managers;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    // DİKKAT: ICacheService cacheService parametresi eklendi!
    public class AppUserService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenService tokenService,
        AppUserManager appUserManager,
        ICacheService cacheService) : IAppUserService
    {
        // Cache için kullanacağımız sabit anahtar kelime
        private const string CacheKey = "all_app_users";

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await unitOfWork.AppUsers.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new BadRequestException("Hatalı şifre.");
            }

            var responseDto = mapper.Map<AuthResponseDto>(user);
            responseDto.Token = tokenService.CreateToken(user);

            return responseDto;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. KURALI ÇALIŞTIR: E-posta benzersiz mi? 
            await appUserManager.CheckIfEmailIsUniqueAsync(registerDto.Email);

            // 2. Kuraldan geçtiyse DTO'yu Entity'e çevir
            var newUser = mapper.Map<AppUser>(registerDto);

            // 3. Şifreyi Hashle
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // 4. Veritabanına ekle
            await unitOfWork.AppUsers.AddAsync(newUser);
            await unitOfWork.SaveChangesAsync();

            // 5. Employee Bağlantısı (İş Akışı)
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

            // YENİ KULLANICI EKLENDİ -> CACHE UÇURULMALI
            await cacheService.RemoveAsync(CacheKey);

            // 6. Kayıt başarılı, token dön
            var responseDto = mapper.Map<AuthResponseDto>(newUser);
            responseDto.Token = tokenService.CreateToken(newUser);

            return responseDto;
        }

        public async Task<List<AppUserDto>> GetAllUsersAsync()
        {
            // 1. Önce Cache'e bak!
            var cachedUsers = await cacheService.GetAsync<List<AppUserDto>>(CacheKey);

            if (cachedUsers != null)
            {
                // Veritabanına gitmedik, RAM'den anında döndük!
                return cachedUsers;
            }

            // 2. Cache'de yoksa Veritabanından (DB) al ve ID'ye göre SIRALA
            var users = await unitOfWork.AppUsers.GetAllAppUsersWithDetailsAsync();
            var sortedList = users.OrderBy(u => u.Id).ToList();
            var dtoList = mapper.Map<List<AppUserDto>>(sortedList);

            // 3. Cache'e kaydet (Örn: 2 saat kalsın)
            await cacheService.SetAsync(CacheKey, dtoList, TimeSpan.FromHours(2));

            return dtoList;
        }

        public async Task<AppUserDto> GetUserByEmailAsync(string email)
        {
            // Tekil sorgulamalarda Cache'e bakmıyoruz, veritabanından güncel veriyi çekiyoruz
            var user = await unitOfWork.AppUsers.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }
            return mapper.Map<AppUserDto>(user);
        }

        public async Task AssignRoleToUserAsync(int userId, string role)
        {
            var user = await unitOfWork.AppUsers.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }

            user.Role = role;
            unitOfWork.AppUsers.Update(user);
            await unitOfWork.SaveChangesAsync();

            // ROL GÜNCELLENDİ -> CACHE UÇURULMALI (Kullanıcı listesi değişti)
            await cacheService.RemoveAsync(CacheKey);
        }
    }
}