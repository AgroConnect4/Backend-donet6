using System;
using System.ComponentModel.DataAnnotations;

namespace agroApp.API.DTOs
{
    public class UpdatePostDto
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
       public List<string> Categories { get; set; } = new List<string>();
    }
}