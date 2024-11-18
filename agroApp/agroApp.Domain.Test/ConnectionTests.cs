/*using Xunit;
using agroApp.Domain.Entities;
using System;

namespace Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void Connection_ShouldHaveValidUserId_WhenSet()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Use Guid.NewGuid()
            var connection = new Connection { UserId = userId };

            // Act
            var actualUserId = connection.UserId;

            // Assert
            Assert.Equal(userId, actualUserId);
        }

         [Fact]
        public void Connection_ShouldThrowArgumentException_WhenUserIdIsInvalid()
        {
            // Arrange
            var connection = new Connection();

            // Act & Assert
            Assert.Throws<FormatException>(() => connection.UserId = Guid.Parse("invalid-guid")); // Testando com um Guid inválido
        }

        [Fact]
        public void Connection_ShouldHaveValidConnectedUserId_WhenSet()
        {
            // Arrange
            var connectedUserId = Guid.NewGuid(); // Use Guid.NewGuid()
            var connection = new Connection { ConnectedUserId = connectedUserId };

            // Act
            var actualConnectedUserId = connection.ConnectedUserId;

            // Assert
            Assert.Equal(connectedUserId, actualConnectedUserId);
        }

        [Fact]
        public void Connection_ShouldThrowArgumentException_WhenConnectedUserIdIsInvalid()
        {
            // Arrange
            var connection = new Connection();

            // Act & Assert
            Assert.Throws<FormatException>(() => connection.ConnectedUserId = Guid.Parse("invalid-guid")); // Testando com um Guid inválido
        }

        [Fact]
        public void Connection_ShouldHaveValidUser()
        {
            // Arrange
            var user = new User();
            var connection = new Connection { User = user };

            // Act
            var actualUser = connection.User;

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public void Connection_ShouldHaveValidConnectedUser()
        {
            // Arrange
            var connectedUser = new User();
            var connection = new Connection { ConnectedUser = connectedUser };

            // Act
            var actualConnectedUser = connection.ConnectedUser;

            // Assert
            Assert.Equal(connectedUser, actualConnectedUser);
        }

        [Fact]
        public void Connection_ShouldThrowArgumentException_WhenUserIdIsEmpty()
        {
            // Arrange
            var connection = new Connection();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => connection.UserId = Guid.Empty); 
        }

        [Fact]
        public void Connection_ShouldThrowArgumentException_WhenConnectedUserIdIsEmpty()
        {
            // Arrange
            var connection = new Connection();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => connection.ConnectedUserId = Guid.Empty); 
        }
    }
}*/