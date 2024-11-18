using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs
{
    public class UpdatePostCommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}