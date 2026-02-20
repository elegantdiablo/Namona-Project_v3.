using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void ShowUsers_Validate()
        {
            var result = _model.ShowUsers().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.UserId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.UserName)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Password)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Role)));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.Email)));
            Assert.All(result, r => Assert.True(r.Phone > 0));
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
            var result = _model.AdminLogin(adminUser.UserName, "adminpassword");
            Assert.NotNull(result);
            Assert.Equal(adminUser.UserName, result.UserName);
            Assert.Equal(adminUser.Role, result.Role);
        }

        [Fact]
        public void Registration_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            _model.Registration(uniqueUsername, password).Wait();
            var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(userInDb);
            Assert.Equal(uniqueUsername, userInDb.UserName);
            Assert.Equal("User", userInDb.Role);
        }

        [Fact]
        public void ValidateUser_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            _model.Registration(uniqueUsername, password).Wait();
            var result = _model.ValidateUser(uniqueUsername, password);
            Assert.NotNull(result);
            Assert.Equal(uniqueUsername, result.UserName);
        }

        [Fact]

        public void DeleteUser_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            _model.Registration(uniqueUsername, password).Wait();
            var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(userInDb);
            _context.users.Remove(userInDb);
            _context.SaveChanges();
            var deletedUser = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.Null(deletedUser);
        }

        [Fact]
        public void UpdateUser_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            _model.Registration(uniqueUsername, password).Wait();
            var userInDb = _context.users.FirstOrDefault(x => x.UserName == uniqueUsername);
            Assert.NotNull(userInDb);
            userInDb.Email = "";

        }
        [Fact]
        public void PromoteToAdmin_Validate()
        {
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";
            var password = "testpassword";
            _model.Registration(uniqueUsername, password).Wait();
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
