/*using Xunit;
using agroApp.Domain.Entities;
using System; // Add this using statement

namespace Tests
{
    public class UserRoleTests
    {
        [Fact]
        public void UserRole_ShouldHaveValidUserId()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Changed to Guid
            var userRole = new UserRole { UserId = userId };

            // Act
            var actualUserId = userRole.UserId;

            // Assert
            Assert.Equal(userId, actualUserId);
        }

        [Fact]
        public void UserRole_ShouldHaveValidRoleId()
        {
            // Arrange
            var roleId = Guid.NewGuid(); // Changed to Guid
            var userRole = new UserRole { RoleId = roleId };

            // Act
            var actualRoleId = userRole.RoleId;

            // Assert
            Assert.Equal(roleId, actualRoleId);
        }

        [Fact]
        public void UserRole_ShouldHaveValidUser()
        {
            // Arrange
            var user = new User();
            var userRole = new UserRole { User = user };

            // Act
            var actualUser = userRole.User;

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public void UserRole_ShouldHaveValidRole()
        {
            // Arrange
            var role = new Role();
            var userRole = new UserRole { Role = role };

            // Act
            var actualRole = userRole.Role;

            // Assert
            Assert.Equal(role, actualRole);
        }
    }
}*/