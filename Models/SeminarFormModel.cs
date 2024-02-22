using Microsoft.AspNetCore.Identity;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models
{
    public class SeminarFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.SeminarConstants.TopicMaxLength,
         MinimumLength = DataConstants.SeminarConstants.TopicMinLength,
         ErrorMessage = "Invalid title length. It must be between {2} and {1} characters.")]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(DataConstants.SeminarConstants.LecturerMaxLength,
         MinimumLength = DataConstants.SeminarConstants.LecturerMinLength,
         ErrorMessage = "Invalid lecturer length. It must be between {2} and {1} characters.")]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(DataConstants.SeminarConstants.DetailsMaxLength,
         MinimumLength = DataConstants.SeminarConstants.DetailsMinLength,
         ErrorMessage = "Invalid details length. It must be between {2} and {1} characters.")]
        public string Details { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        
        [Range(DataConstants.SeminarConstants.DurationMinLength 
          , DataConstants.SeminarConstants.DurationMaxLength)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        public List<Category> Categories { get; internal set; } = new List<Category>();
    }
}
