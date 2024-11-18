/*using Xunit;
using agroApp.Domain.Entities;

namespace Tests
{
    public class BlogPostTests
    {
        [Fact]
        public void BlogPost_ShouldHaveValidTitle()
        {
            // Arrange
            var title = "Título do Post";
            var blogPost = new BlogPost { Title = title };

            // Act
            var actualTitle = blogPost.Title;

            // Assert
            Assert.Equal(title, actualTitle);
        }

        [Fact]
        public void BlogPost_ShouldHaveValidContent()
        {
            // Arrange
            var content = "Conteúdo do post...";
            var blogPost = new BlogPost { Content = content };

            // Act
            var actualContent = blogPost.Content;

            // Assert
            Assert.Equal(content, actualContent);
        }

        [Fact]
        public void BlogPost_ShouldHaveValidDateCreated()
        {
            // Arrange
            var dateCreated = DateTime.Now;
            var blogPost = new BlogPost { DateCreated = dateCreated };

            // Act
            var actualDateCreated = blogPost.DateCreated;

            // Assert
            Assert.Equal(dateCreated, actualDateCreated);
        }

        [Fact]
        public void BlogPost_ShouldHaveValidUserId()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Create a new GUID
            var blogPost = new BlogPost { UserId = userId };

            // Act
            var actualUserId = blogPost.UserId;

            // Assert
            Assert.Equal(userId, actualUserId);
        }

        [Fact]
        public void BlogPost_ShouldHaveValidUser()
        {
            // Arrange
            var user = new User();
            var blogPost = new BlogPost { User = user };

            // Act
            var actualUser = blogPost.User;

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public void BlogPost_ShouldNotHaveEmptyTitle()
        {
            // Arrange
            var blogPost = new BlogPost(); 

            // Act & Assert
            Assert.Throws<ArgumentException>(() => blogPost.Title = ""); 
        }

        [Fact]
        public void BlogPost_ShouldNotHaveEmptyContent()
        {
            // Arrange
            var blogPost = new BlogPost(); 

            // Act & Assert
            Assert.Throws<ArgumentException>(() => blogPost.Content = ""); 
        }
    }
}*/