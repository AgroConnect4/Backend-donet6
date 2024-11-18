using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs
{
    public class SharePostDto
    {
        [Required]
        public Guid PostId { get; set; }
    }
}