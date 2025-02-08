using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos
{
    public class WorkoutDayDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public Guid fk_routine { get; set; }

        public List<Guid>? fk_exercises { get; set; }
    }
}
