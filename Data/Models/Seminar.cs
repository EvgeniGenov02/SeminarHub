using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;

namespace SeminarHub.Data.Models
{
    public class Seminar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.SeminarConstants.TopicMaxLength)]
        public string Topic { get; set; } = null!;

        [Required]
        [MaxLength(DataConstants.SeminarConstants.LecturerMaxLength)]
        public string Lecturer { get; set; } = null !;

        [Required]
        [MaxLength(DataConstants.SeminarConstants.DetailsMaxLength)]
        public string Details { get; set; } = null!;

        [Required]
        public string OrganizerId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [MaxLength(DataConstants.SeminarConstants.DurationMaxLength)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        public IEnumerable<SeminarParticipant> SeminarsParticipants { get; set; }
        = new List<SeminarParticipant>();
    }
    }
