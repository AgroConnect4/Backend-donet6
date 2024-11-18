using agroApp.Domain.Entities;

namespace agroApp.API.DTOs
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Guid OrganizerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> ProductImages { get; set; }
    }
}