using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs
{
    public class ShareEventDto
    {
        [Required]
        public Guid EventId { get; set; }
    }
}