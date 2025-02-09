using System.ComponentModel.DataAnnotations;

namespace GymTracker.Server.Dtos
{
    public class WorkoutDto
    {
        public Guid? Id { get; set; }

        public DateTime Date { get; set; }

        public string? Observations {  get; set; }

        public Guid fk_workoutday { get; set; }

        public List<ExerciseExecution> exerciseExecutions = new List<ExerciseExecution> ();
    }
}
