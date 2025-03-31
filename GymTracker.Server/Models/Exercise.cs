using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        [StringLength(500)]
        public string? description { get; set; }

        [Required]
        public Guid fk_bodypart { get; set; }

        [ForeignKey("fk_bodypart")]
        public BodyPart bodyPart { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime? updatedAt { get; set; }

        public bool isDeleted { get; set; } = false;
    }
}
