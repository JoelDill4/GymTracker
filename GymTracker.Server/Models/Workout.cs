using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
namespace GymTracker.Server.Models
{
    public class Workout
    {
        public Workout() { }

        public Workout(DateTime date, string? observations, Guid workoutDayId, WorkoutDay workoutDay)
        {
            this.Date = date;
            this.Observations = observations;
            this.WorkoutDayId = workoutDayId;
            this.WorkoutDay = workoutDay;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(1000)]
        public string? Observations { get; set; }

        [Required]
        public Guid WorkoutDayId { get; set; }

        [ForeignKey("WorkoutDayId")]
        public virtual WorkoutDay WorkoutDay { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}*/