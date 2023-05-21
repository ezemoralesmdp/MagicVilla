using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUserUnique(string userName)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            return user == null;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower() && u.Password == loginRequestDto.Password);

            if(user == null) return new LoginResponseDto()
            {
                Token = "",
                User = null
            };

            //Si el usuario existe generamos un JW Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return loginResponseDto;
        }

        public async Task<User> Register(RegisterRequestDto registerRequestDto)
        {
            User user = new()
            {
                UserName = registerRequestDto.UserName,
                Password = registerRequestDto.Password,
                Name = registerRequestDto.Name,
                Rol = registerRequestDto.Rol
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user; 
        }
    }
}
