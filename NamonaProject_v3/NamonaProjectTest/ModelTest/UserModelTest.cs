using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using NamonaProject_v3_.DTO;

namespace NamonaProjectTest.ModelTest
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
        public async Task Register_Should_Add_New_User()
        {
            // Arrange
            var dto = new RegistrationDto
            {
                Email = "newuser@test.com",
                UserName = "newuser",
                Password = "password123"
            };

            // Act
            await _model.Register(dto);

            // Assert
            var user = _context.users.FirstOrDefault(u => u.Email == dto.Email);
            Assert.NotNull(user);
            Assert.Equal("newuser", user.UserName);
            Assert.Equal("User", user.Role);
            Assert.NotEqual("password123", user.Password); // should be hashed
        }

        [Fact]
        public async Task Register_Should_Throw_If_Email_Exists()
        {
            // Arrange
            var existing = _context.users.First();

            var dto = new RegistrationDto
            {
                Email = existing.Email,
                UserName = "duplicate",
                Password = "123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _model.Register(dto));
        }

        [Fact]
        public async Task ValidateUser_Should_Return_UserDto_When_Correct()
        {
            // Arrange
            var dto = new RegistrationDto
            {
                Email = "login@test.com",
                UserName = "loginuser",
                Password = "pass123"
            };

            await _model.Register(dto);

            // Act
            var result = await _model.ValidateUser(dto.Email, dto.Password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task ValidateUser_Should_Throw_When_Invalid()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _model.ValidateUser("wrong@test.com", "wrong"));
        }

        [Fact]
        public void ShowUsers_Should_Return_Users()
        {
            // Act
            var users = _model.ShowUsers().ToList();

            // Assert
            Assert.NotEmpty(users);
            Assert.All(users, u => Assert.NotNull(u.UserName));
        }

        [Fact]
        public async Task AdminLogin_Should_Return_Admin_When_Correct()
        {
            // Arrange
            var dto = new RegistrationDto
            {
                Email = "admin@test.com",
                UserName = "adminuser",
                Password = "adminpass"
            };

            await _model.Register(dto);

            var user = _context.users.First(u => u.Email == dto.Email);
            user.Role = "Admin";
            await _context.SaveChangesAsync();

            // Act
            var result = await _model.AdminLogin(dto.UserName, dto.Password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.Role);
        }

        [Fact]
        public async Task AdminLogin_Should_Throw_When_Not_Admin()
        {
            // Arrange
            var dto = new RegistrationDto
            {
                Email = "user@test.com",
                UserName = "normaluser",
                Password = "pass"
            };

            await _model.Register(dto);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _model.AdminLogin(dto.UserName, dto.Password));
        }

        [Fact]
        public async Task DeleteUser_Should_Remove_User()
        {
            // Arrange
            var user = _context.users.First();

            // Act
            await _model.DeleteUser(user.UserId);

            // Assert
            Assert.False(_context.users.Any(u => u.UserId == user.UserId));
        }

        [Fact]
        public async Task DeleteUser_Should_Throw_When_NotFound()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _model.DeleteUser(999999));
        }

        [Fact]
        public async Task UpdatePassword_Should_Change_Password()
        {
            // Arrange
            var user = _context.users.First();
            var oldPassword = user.Password;

            // Act
            await _model.UpdatePassword(user.UserId, "newpassword");

            // Assert
            var updated = _context.users.First(u => u.UserId == user.UserId);
            Assert.NotEqual(oldPassword, updated.Password);
        }

        [Fact]
        public async Task PromoteToAdmin_Should_Set_Role()
        {
            // Arrange
            var user = _context.users.First();

            // Act
            await _model.PromoteToAdmin(user.UserId);

            // Assert
            var updated = _context.users.First(u => u.UserId == user.UserId);
            Assert.Equal("Admin", updated.Role);
        }

        [Fact]
        public async Task GetByEmail_Should_Return_User()
        {
            // Arrange
            var user = _context.users.First();

            // Act
            var result = await _model.GetByEmail(user.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmail_Should_Return_Null_When_NotFound()
        {
            var result = await _model.GetByEmail("notfound@test.com");
            Assert.Null(result);
        }

        [Fact]
        public async Task EditUser_Should_Update_User()
        {
            // Arrange
            var user = _context.users.First();

            var dto = new UserDto
            {
                UserId = user.UserId,
                UserName = "updated",
                Email = "updated@test.com",
                Phone = "123456",
                Role = "User"
            };

            // Act
            await _model.EditUser(dto);

            // Assert
            var updated = _context.users.First(u => u.UserId == user.UserId);
            Assert.Equal("updated", updated.UserName);
            Assert.Equal("updated@test.com", updated.Email);
        }

        [Fact]
        public async Task EditUser_Should_Throw_When_Invalid_Data()
        {
            var user = _context.users.First();

            var dto = new UserDto
            {
                UserId = user.UserId,
                UserName = null,
                Email = null,
                Phone = null,
                Role = null
            };

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _model.EditUser(dto));
        }
    }
}