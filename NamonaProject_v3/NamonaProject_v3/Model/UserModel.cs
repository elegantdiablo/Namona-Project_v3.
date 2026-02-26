using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using System.Security.Cryptography;
using System.Text;

namespace NamonaProject_v3_.Model
{
    public class UserModel
    {
        public NamonaDbContext _context;
        public UserModel(NamonaDbContext context)
        {
            _context = context;
        }

        public async Task Registration(string email, string username, string password, string role = "User")
        {
            if (_context.users.Any(x => x.Email == email))
            {
                throw new InvalidOperationException("already exists");
            }
            using var trx = _context.Database.BeginTransaction();
            _context.users.Add(new Users { Email = email, Password = HashPassword(password), Role = role, UserName = username });
            _context.SaveChanges();
            trx.Commit();
            await Task.CompletedTask;
        }

        public async Task<UserDto> ValidateUser(string email, string password)
        {
            var hash = HashPassword(password);
            var user = _context.users.Where(x => x.Email == email);
            return user.Where(x => x.Password == hash).Select(x => new UserDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Role = x.Role
            }).First();
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
            return _context.users.OrderBy(x => x.UserName).Select(x => new UserDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.PhoneNumber != null ? x.PhoneNumber : "",
                Role = x.Role
            });
        }

        public UserDto? AdminLogin(string email, string password)
        {
            var hash = HashPassword(password);
            var user = _context.users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Role == "Admin");
            if (user == null || user.Password != hash)
            return null;
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role
            };
        }

        public async Task DeleteUser(int userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            using var trx = _context.Database.BeginTransaction();
            _context.users.Remove(user);
            _context.SaveChanges();
            trx.Commit();
            await Task.CompletedTask;
        }
        public async Task UpdatePassword(int userId, string newPassword)
        {
            var user = _context.users.Find(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            using var trx = _context.Database.BeginTransaction();
            user.Password = HashPassword(newPassword);
            _context.SaveChanges();
            trx.Commit();
            await Task.CompletedTask;
        }

        public async Task PromoteToAdmin(int userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            using var trx = _context.Database.BeginTransaction();
            user.Role = "Admin";
            _context.SaveChanges();
            trx.Commit();
            await Task.CompletedTask;
        }
    }
}