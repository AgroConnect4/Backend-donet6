namespace agroApp.API.DTOs;

public class UpdateEventDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> ProductImages { get; set; }
    }