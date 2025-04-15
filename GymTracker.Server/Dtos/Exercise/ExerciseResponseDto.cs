using GymTracker.Server.Dtos.BodyPart;
using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Exercise
{
    public class ExerciseResponseDto
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string? description { get; set; }

        public BodyPartDto bodyPart { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
} 