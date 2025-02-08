using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos
{
    public class ExerciseDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public List<Guid>? fk_workoutdays { get; set; }

        public List<Guid>? fk_bodyparts { get; set; }
    }
}
