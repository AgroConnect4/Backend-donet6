/*using Xunit;
using agroApp.Domain.Entities;

namespace Tests
{
    public class RoleTests
    {
        [Fact]
        public void Role_ShouldHaveValidName()
        {
            // Arrange
            var name = "Administrador";
            var role = new Role { Name = name };

            // Act
            var actualName = role.Name;

            // Assert
            Assert.Equal(name, actualName);
        }

        [Fact]
        public void Role_ShouldHaveListOfUserRoles()
        {
            // Arrange
            var role = new Role();

            // Act
            var userRoles = role.UserRoles;

            // Assert
            Assert.NotNull(userRoles);
        }

        [Fact]
        public void Role_ShouldNotHaveEmptyName()
        {
            // Arrange
            var role = new Role();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => role.Name = ""); 
        }
    }
}*/