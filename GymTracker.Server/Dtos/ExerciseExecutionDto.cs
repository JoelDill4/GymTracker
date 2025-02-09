namespace GymTracker.Server.Dtos
{
    public class ExerciseExecutionDto
    {
        public Guid ExerciseId { get; set; }

        public string? ExerciseName { get; set; }

        public List<int> Weights { get; set; }

        public List<int> Reps { get; set; }
    }
}
