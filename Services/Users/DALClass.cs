using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Users
{
    public class DALClass : IUser
    {
        private readonly EsicPfRegistrationDbContext _context;
        private readonly IConfiguration _config;

        public DALClass(EsicPfRegistrationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserDTO?> LoginAsync(string username, string password)
        {

            var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isValid)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };

        }

        public async Task<bool> RegisterUserAsync(UserDTO model)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Username = model.Username,
                Password = hashedPassword,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
