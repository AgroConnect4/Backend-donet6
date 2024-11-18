using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace agroApp.Domain.Entities
{
    public class Post
    {   
        
        public Guid Id { get; set; }

        // Chave estrangeira para User
        public Guid UserId { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime? EditedAt { get; set; }
        
        // Propriedade de navegação para User
        public User User { get; set; }

        public int LikesCount { get; set; }

        public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
        public ICollection<PostShare> Shares { get; set; } = new List<PostShare>();

        public List<string> Categories { get; set; } = new List<string>();
        
    }
}
