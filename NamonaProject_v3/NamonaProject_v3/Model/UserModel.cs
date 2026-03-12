using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using System.Security.Cryptography;
using System.Text;

namespace NamonaProject_v3_.Model
{
    public class UserModel
    {
        private readonly NamonaDbContext _context;

        public UserModel(NamonaDbContext context)
        {
            _context = context;
        }

        public async Task Register(RegistrationDto dto)
        {
            var existingUser = await _context.users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");
            var user = new Users
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = HashPassword(dto.Password),
                Role = "User"   
            };

            _context.users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> ValidateUser(string email, string password)
        {
            var hash = HashPassword(password);

            var user = await _context.users
                .FirstOrDefaultAsync(x => x.Email == email && x.Password == hash);

            if (user == null)
                throw new InvalidOperationException("Invalid email or password");

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public IEnumerable<UserDto> ShowUsers()
        {
            return _context.users
                .OrderBy(x => x.UserName)
                .Select(x => new UserDto
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    Phone = x.PhoneNumber ?? "",
                    Role = x.Role
                });
        }

        public UserDto? AdminLogin(string email, string password)
        {
            var hash = HashPassword(password);

            var user = _context.users
                .FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Role == "Admin");

            if (user == null || user.Password != hash)
                return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            using var trx = await _context.Database.BeginTransactionAsync();

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            await trx.CommitAsync();
        }

        public async Task UpdatePassword(int userId, string newPassword)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            using var trx = await _context.Database.BeginTransactionAsync();

            user.Password = HashPassword(newPassword);

            await _context.SaveChangesAsync();
            await trx.CommitAsync();
        }

        public async Task PromoteToAdmin(int userId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            using var trx = await _context.Database.BeginTransactionAsync();

            user.Role = "Admin";

            await _context.SaveChangesAsync();
            await trx.CommitAsync();
        }
        public async Task<UserDto?> GetByEmail(string email)
        {
            var user = await _context.users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}