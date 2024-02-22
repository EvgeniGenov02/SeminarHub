using Microsoft.AspNetCore.Identity;
using SeminarHub.Data.Models;

namespace SeminarHub.Models
{
    public class SeminarViewModel
    {
        public SeminarViewModel(
            int id, 
            string topic,
            string lecturer,
            string details,
            string organizerId,
            IdentityUser organizer,
            string dateAndTime,
            int duration,
            int categoryId,
            Category category
            )
        {
            this.Id = id;
            this.Topic = topic;
            this.Lecturer = lecturer;
            this.Details = details;
            this.OrganizerId = organizerId;
            this.Organizer = organizer;
            this.DateAndTime = dateAndTime;
            this.Duration = duration;
            this.CategoryId = categoryId;
            this.Category = category;
        }
        public int Id { get; set; }

        public string Topic { get; set; } 

        public string Lecturer { get; set; } 

        public string Details { get; set; } 

        public string OrganizerId { get; set; } 

        public IdentityUser Organizer { get; set; } 

        public string DateAndTime { get; set; }

        public int Duration { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } 
    }
}
