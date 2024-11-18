/*using Xunit;
using agroApp.Domain.Entities;

namespace Tests
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldHaveValidEmail()
        {
            // Arrange
            var email = "teste@email.com";
            var user = new User { Email = email };

            // Act
            var actualEmail = user.Email;

            // Assert
            Assert.Equal(email, actualEmail);
        }

        [Fact]
        public void User_ShouldHaveProfile()
        {
            // Arrange
            var user = new User();
            user.Profile = new Profile(); // Inicialize a propriedade Profile

            // Act
            var profile = user.Profile;

            // Assert
            Assert.NotNull(profile);
        }

        [Fact]
        public void User_ShouldHaveListOfPosts()
        {
            // Arrange
            var user = new User();

            // Act
            var posts = user.Posts;

            // Assert
            Assert.NotNull(posts);
        }

        [Fact]
        public void User_ShouldHaveListOfUserRoles()
        {
            // Arrange
            var user = new User();

            // Act
            var userRoles = user.UserRoles;

            // Assert
            Assert.NotNull(userRoles);
        }

        [Fact]
        public void User_ShouldNotHaveEmptyEmail()
        {
            // Arrange
            var user = new User();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => user.Email = "");
        }

        [Fact]
        public void User_ShouldNotHaveInvalidEmailFormat()
        {
            // Arrange
            var user = new User();

            // Act & Assert
            Assert.Throws<FormatException>(() => user.Email = "invalid-email");
        }

        [Fact]
        public void User_ShouldHaveProfileInitialized()
        {
            // Arrange
            var user = new User();
            user.Profile = new Profile(); // Inicialize a propriedade Profile

            // Act
            var profile = user.Profile;

            // Assert
            Assert.NotNull(profile);
        }
    }
}*/