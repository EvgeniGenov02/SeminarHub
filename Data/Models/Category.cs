using Humanizer;
using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SeminarHub.Data.Models
{
    public class Category
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CategoryConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<Seminar> Seminars { get; set; } 
           = new List<Seminar>();
    }
}