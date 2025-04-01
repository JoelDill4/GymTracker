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
        [StringLength(100)]
        public string name { get; set; } = string.Empty;

        [StringLength(500)]
        public string description { get; set; } = string.Empty;

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isDeleted { get; set; }
    }
}