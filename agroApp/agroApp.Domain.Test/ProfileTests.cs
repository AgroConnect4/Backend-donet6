/*using Xunit;
using agroApp.Domain.Entities;
using System;

namespace Tests
{
    public class ProfileTests
    {
        [Fact]
        public void Profile_ShouldHaveValidBio()
        {
            // Arrange
            var bio = "Minha biografia...";
            var profile = new Profile { Bio = bio };

            // Act
            var actualBio = profile.Bio;

            // Assert

            Assert.Equal(bio, actualBio);
        }

        [Fact]
        public void Profile_ShouldHaveValidProfilePicture()
        {
            // Arrange
            var profilePicture = "https://example.com/profile.jpg";
            var profile = new Profile { ProfilePicture = profilePicture };

            // Act
            var actualProfilePicture = profile.ProfilePicture;

            // Assert
            Assert.Equal(profilePicture, actualProfilePicture);
        }

        [Fact]
        public void Profile_ShouldHaveValidCoverPicture()
        {
            // Arrange
            var coverPicture = "https://example.com/cover.jpg";
            var profile = new Profile { CoverPicture = coverPicture };

            // Act
            var actualCoverPicture = profile.CoverPicture;

            // Assert
            Assert.Equal(coverPicture, actualCoverPicture);
        }

        [Fact]
        public void Profile_ShouldHaveValidDescription()
        {
            // Arrange
            var description = "Descrição do perfil...";
            var profile = new Profile { Description = description };

            // Act
            var actualDescription = profile.Description;

            // Assert
            Assert.Equal(description, actualDescription);
        }

        [Fact]
        public void Profile_ShouldHaveValidUserId()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Use Guid.NewGuid()
            var profile = new Profile { UserId = userId };

            // Act
            var actualUserId = profile.UserId;

            // Assert
            Assert.Equal(userId, actualUserId);
        }

        [Fact]
        public void Profile_ShouldHaveValidUser()
        {
            // Arrange
            var user = new User();
            var profile = new Profile { User = user };

            // Act
            var actualUser = profile.User;

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public void Profile_ShouldNotHaveNullBio()
        {
            // Arrange
            var profile = new Profile(); // Crie sem atribuir valor inicial

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => profile.Bio = null);
        }

        [Fact]
        public void Profile_ShouldNotHaveInvalidProfilePictureUrl()
        {
            // Arrange
            var profile = new Profile();

            // Act & Assert
            Assert.Throws<FormatException>(() => profile.ProfilePicture = "invalid-url");
        }
    }
}*/