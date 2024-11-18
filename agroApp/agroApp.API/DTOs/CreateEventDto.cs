using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs;
public class CreateEventDto
    {
        [Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        //public Guid userId { get; set; } 
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        public List<string> ProductImages { get; set; }
    }
