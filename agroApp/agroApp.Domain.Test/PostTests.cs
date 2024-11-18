/*using Xunit;
using agroApp.Domain.Entities;

namespace Tests
{
    public class PostTests
    {
        [Fact]
        public void Post_ShouldHaveValidContent()
        {
            // Arrange
            var content = "Conteúdo do post...";
            var post = new Post { Content = content };

            // Act
            var actualContent = post.Content;

            // Assert
            Assert.Equal(content, actualContent);
        }

        

        [Fact]
        public void Post_ShouldHaveValidCreatedAt()
        {
            // Arrange
            var createdAt = DateTime.Now;
            var post = new Post { CreatedAt = createdAt };

            // Act
            var actualCreatedAt = post.CreatedAt;

            // Assert
            Assert.Equal(createdAt, actualCreatedAt);
        }

        [Fact]
        public void Post_ShouldHaveValidPostType_WhenSet()
        {
            // Arrange
            var postTypeId = Guid.NewGuid();
            var postType = new PostType { Id = postTypeId, Name = "Produtos" };
            var post = new Post { PostType = postType, PostTypeId = postTypeId };

            // Act
            var actualPostType = post.PostType;

            // Assert
            Assert.Equal(postType, actualPostType);
        }

        [Fact]
        public void Post_ShouldHaveValidUserId()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Use Guid.NewGuid()
            var post = new Post { UserId = userId };

            // Act
            var actualUserId = post.UserId;

            // Assert
            Assert.Equal(userId, actualUserId);
        }

        [Fact]
        public void Post_ShouldHaveValidUser()
        {
            // Arrange
            var user = new User();
            var post = new Post { User = user };

            // Act
            var actualUser = post.User;

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public void Post_ShouldNotHaveEmptyContent()
        {
            // Arrange
            var post = new Post(); // Crie sem atribuir valor inicial

            // Act & Assert
            Assert.Throws<ArgumentException>(() => post.Content = ""); 
        }

        [Fact]
        public void Post_ShouldThrowArgumentException_WhenPostTypeIsInvalid()
        {
            // Arrange
            var post = new Post();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => post.PostType = "InvalidType"); 
        }
        
    }
}*/