using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class BodyPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime? updatedAt { get; set; }

        public bool isDeleted { get; set; } = false;
    }
} 