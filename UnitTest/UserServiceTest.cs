using AutomixMVC.Data;
using AutomixMVC.Models;
using AutomixMVC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTest
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AutomixDbContext? _context;

        public UserServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();

            var options = new DbContextOptionsBuilder<AutomixDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AutomixDbContext(options, _configurationMock.Object);

            _passwordHasherMock = new Mock<IPasswordHasher>();
            _userService = new UserService(_passwordHasherMock.Object, context);
        }
        protected void Dispose() // Cleanup after each test
        {
            if (_context != null)
            {
                _context.Database.EnsureDeleted(); // Ensure DB is deleted after test
            }
            _passwordHasherMock.Reset(); // Reset mock to default state
        }


        [Fact]
        public async Task Authenticate_ReturnsUser_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User("admin", "someHashedPassword", "Admin");

            _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");

            await _userService.CreateUser(user.Username, "password", user.Role ?? "DefaultRole");

            _passwordHasherMock.Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true); // This assumes that the password is always correct for the purpose of the test.

            // Act
            var result = await _userService.Authenticate(user.Username, "password");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
        }
        //Test for invalid credentials : This is the opposite scenario of the previous one. If the provided credentials are incorrect, Authenticate() should return null.

        [Fact]
        public async Task Authenticate_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var user = new User("admin", "someHashedPassword", "Admin");

            _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");

            await _userService.CreateUser(user.Username, "password", user.Role ?? "DefaultRole");

            _passwordHasherMock.Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false); // This assumes that the password is always incorrect for the purpose of the test.

            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _userService.Authenticate(user.Username, "wrongpassword"));

            // Assert
            Assert.Equal("Invalid username or password", ex.Message);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser()
        {
            // Arrange
            var user = new User("admin", "someHashedPassword", "Admin");
            var password = "password";

            _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");

            // Act
            var result = await _userService.CreateUser(user.Username, password, user.Role ?? "DefaultRole");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Role, result.Role);
            Assert.Equal("hashedpassword", result.PasswordHash);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User("admin1", "someHashedPassword", "Admin");
            var user2 = new User("admin2", "someHashedPassword", "Admin");

            _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hashedpassword");

            await _userService.CreateUser(user1.Username, "password1", user1.Role ?? "DefaultRole");
            await _userService.CreateUser(user2.Username, "password2", user2.Role ?? "DefaultRole");

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

    }
}
