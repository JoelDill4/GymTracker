using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos.Exercise
{
    public class BodyPartDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
} 