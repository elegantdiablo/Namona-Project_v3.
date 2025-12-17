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

        public void Registration(string username, string password, string role = "User")
        {
            if (_context.users.Any(x => x.UserName == username))
            {
                throw new InvalidOperationException("already exists");
            }
            using var trx = _context.Database.BeginTransaction();
            _context.users.Add(new Users { UserName = username, Password = HashPassword(password), Role = role });
            _context.SaveChanges();
            trx.Commit();
        }

        public Users? ValidateUser(string username, string password)
        {
            var hash = HashPassword(password);
            var user = _context.users.Where(x => x.UserName == username);
            return user.Where(x => x.Password == hash).FirstOrDefault();
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
                Password = x.Password
            });
        }

        //Role Modositas
        // Jelszó modositas
    }
}
