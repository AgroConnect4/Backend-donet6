using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace agroApp.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> ProductImages { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<EventComment> Comments { get; set; } = new List<EventComment>();
        public ICollection<EventShare> Shares { get; set; } = new List<EventShare>();
    }
}
