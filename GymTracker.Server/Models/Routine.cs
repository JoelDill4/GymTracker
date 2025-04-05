using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymTracker.Server.Models
{
    public class Routine
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; } = string.Empty;

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}