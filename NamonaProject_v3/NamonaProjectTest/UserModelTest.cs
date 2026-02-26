using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

namespace NamonaProjectTest
{
    public class UserModelTest
    {
        private readonly UserModel _model;
        private readonly NamonaDbContext _context;

        public UserModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new UserModel(_context);
        }
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        [Fact]
        public void ShowUsers_Validate()
        {
            var result = _model.ShowUsers().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.UserId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.UserName)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Role)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Email)));
            Assert.All(result, r => Assert.True(r.Phone is not null));
            Assert.Equal(result.OrderBy(r => r.UserName).Select(r => r.UserName), result.Select(r => r.UserName));
            if (result.Count >= 2)
            {
                Assert.NotEqual(result[0].UserId, result[1].UserId);
                Assert.True(string.Compare(result[0].UserName, result[1].UserName) <= 0);
            }
        }

        [Fact]
        public void AdminLogin_Validate()
        {
            var adminUser = _context.users.FirstOrDefault(x => x.Role == "Admin");
            if (adminUser == null)
            {
                Assert.True(false, "No admin user found in the database.");
                return;
            }
            var result = _model.AdminLogin("admin", "admin123");
            Assert.NotNull(result);
            Assert.Equal(adminUser.UserName, result.UserName);
            Assert.Equal(adminUser.Role, result.Role);
        }

        [Fact]
        public async Task Registration_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            await _model.Registration("asd@gmail.com", uniqueUsername, password);
            var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(userInDb);
            Assert.Equal(uniqueUsername, userInDb.UserName);
            Assert.Equal("User", userInDb.Role);
        }

        [Fact]
        public async Task ValidateUser_Validate()
        {
            var uniqueUsername = "testuser";
            var password = "user123";
           var result = await _model.ValidateUser("user@namona.hu", password);
            Assert.NotNull(result);
            Assert.Equal(uniqueUsername, result.UserName);
        }
        [Fact]
        public async Task ValidateUser_Exits()
        {
            var uniqueUsername = "testuser";
            var password = "testpassword";
            
          await  Assert.ThrowsAsync<InvalidOperationException>(() => _model.Registration("user@namona.hu", uniqueUsername, password));
          
        }

        [Fact]

        public async Task DeleteUser_Validate()
        {


            var user = _context.users.First();
            int userid = user.UserId;

            await _model.DeleteUser(userid);

            var deleted = _context.users
                .FirstOrDefault(x => x.UserId == userid);

            Assert.Null(deleted);

        }

        /*  [Fact]
          public async Task UpdateUser_Validate()
          {
              var uniqueUsername = $"testuser_{Guid.NewGuid()}";
              var password = "testpassword";
              await _model.Registration(uniqueUsername, password);
              var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
              Assert.NotNull(userInDb);
              userInDb.Email = "";

          }*/
        [Fact]
        public async Task PromoteToAdmin_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            await _model.Registration("adminadmin@namona.hu", uniqueUsername, password);
            var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(userInDb);
            userInDb.Role = "Admin";
            _context.SaveChanges();
            var promotedUser = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(promotedUser);
            Assert.Equal("Admin", promotedUser.Role);
        }
    }
}
