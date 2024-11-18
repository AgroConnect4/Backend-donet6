using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs
{
    public class CreateEventCommentDto
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}