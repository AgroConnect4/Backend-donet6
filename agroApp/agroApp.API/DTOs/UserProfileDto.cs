using System.Collections.Generic;

namespace agroApp.API.DTOs
{
    public class UserProfileDto
    {
       public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; } // Adicione o campo Name
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public string CoverPicture { get; set; }
        public string Description { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string WhatsAppNumber { get; set; }
        public List<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
    }
}