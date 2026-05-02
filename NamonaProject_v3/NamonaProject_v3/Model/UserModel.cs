using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using System.Security.Authentication;
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
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
            {
                throw new InvalidDataException();
            }
            if (_context.users.Any(x => x.Email == dto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }
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
                    //Phone = x.PhoneNumber ?? "",
                    Phone = x.PhoneNumber,
                    Role = x.Role
                });
        }

        public async Task<UserDto?> AdminLogin(string username, string password)
        {
            var hash = HashPassword(password);

            var user = _context.users
                .FirstOrDefault(x => x.UserName.ToLower() == username.ToLower() && x.Role == "Admin");

            if (user == null || user.Password != hash)
            {
                throw new KeyNotFoundException();
            }


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
                throw new KeyNotFoundException();

            using (var trx = _context.Database.BeginTransaction())
            {
                _context.users.Remove(user);
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task UpdatePassword(UpdatePasswordDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new InvalidDataException("New password cannot be empty");
            }

            using (var trx = _context.Database.BeginTransaction())
            {
                _context.users.Where(x => x.UserId == dto.UserId).First().Password = HashPassword(dto.Password);

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
            
        }

        public async Task PromoteToAdmin(PromoteDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Role))
            {
                throw new InvalidDataException();
            }
            if(dto.Role != "Admin" && dto.Role != "User")
            {
                throw new InvalidCredentialException();
            }

            using (var trx = _context.Database.BeginTransaction())
            {
                _context.users.Where(x => x.UserId == dto.UserId).First().Role = dto.Role;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
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

        public async Task<UserDto?> GetByUserName(string userName)
        {
            var user = await _context.users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }


        public async Task EditUser(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Role) || string.IsNullOrWhiteSpace(dto.Phone))
            {
                throw new InvalidDataException();
            }
            if (_context.users.Any(x => x.Email == dto.Email))
            {
                throw new InvalidOperationException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.users.Where(x => x.UserId == dto.UserId).First().UserName = dto.UserName;
                _context.users.Where(x => x.UserId == dto.UserId).First().PhoneNumber = dto.UserName;
                _context.users.Where(x => x.UserId == dto.UserId).First().Email = dto.Email;
                _context.users.Where(x => x.UserId == dto.UserId).First().Role = dto.Role;
                await trx.CommitAsync();
                await _context.SaveChangesAsync();
            }
            await Task.CompletedTask;
        }
    }
}