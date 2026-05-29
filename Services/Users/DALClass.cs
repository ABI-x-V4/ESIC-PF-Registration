using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.User;

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
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;

            //bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            //if (!isValid)
            //    return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                CreatedDate = user.CreatedDate
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
