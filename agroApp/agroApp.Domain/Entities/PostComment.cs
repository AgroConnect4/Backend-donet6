using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agroApp.Domain.Entities
{
    public class PostComment : CommentBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        //public Event Event { get; set; } 

        //public Guid CommentableId { get; set; }
        
        //public Guid CommentableType { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}