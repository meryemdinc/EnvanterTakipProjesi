using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Infrastructure.Services
{
   public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user)
        {
            //AppSettings.json dosyasından JWT ayarlarını al
            var jwtSettings =_configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            //UTF8 formatında secretKey'i byte array'e çevir ve SymmetricSecurityKey oluştur
          if(secretKey == null)
            {
                throw new Exception("JWT Secret Key is not configured.");
            }
          //simetrik: aynı anahtar hem şifreleme hem de şifre çözmek için kullanılması
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            //şifreleme algoritması seç
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //  CLAIMS (Kullanıcının Valizi): Token'ın içine gömeceğimiz bilgiler.
            // Bu bilgileri Frontend okuyup ekrana "Hoş geldin Ali" yazabilir.
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role), // Kullanıcının Rolü (Admin/User vs.)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //  Token'ın yaşam süresini ve kime ait olduğunu ayarlayarak iskeletini oluşturuyoruz
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"]!)),
                signingCredentials: creds
            );

            //  İskeleti string metne çevirip (üretip) geri döndürüyoruz!
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
