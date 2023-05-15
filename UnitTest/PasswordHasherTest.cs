using AutomixMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class PasswordHasherTest
    {   
        private IPasswordHasher _passwordHasher = new PasswordHasher();
        [Fact]
        public void HashPassword_GivenPassword_ReturnsHashedPassword()
        {
            // Arrange
            string password = "MyTestPassword";

            // Act
            string hashedPassword = _passwordHasher.HashPassword(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_GivenValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "MyTestPassword";
            string hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            bool isVerified = _passwordHasher.VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyPassword_GivenInvalidPassword_ReturnsFalse()
        {
            // Arrange
            string password = "MyTestPassword";
            string hashedPassword = _passwordHasher.HashPassword(password);

            // Act
            bool isVerified = _passwordHasher.VerifyPassword(hashedPassword, "WrongPassword");

            // Assert
            Assert.False(isVerified);
        }
    }
}
