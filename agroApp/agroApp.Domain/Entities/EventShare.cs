using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agroApp.Domain.Entities
{
    public class EventShare : ShareBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public Event Event { get; set; }

        //public Guid CommentableType { get; set; } 

        //public Guid CommentableId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime SharedAt { get; set; }
    }
}