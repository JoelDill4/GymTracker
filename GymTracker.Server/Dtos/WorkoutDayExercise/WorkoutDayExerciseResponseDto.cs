using GymTracker.Server.Dtos.BodyPart;

namespace GymTracker.Server.Dtos.WorkoutDayExercise
{
    public class WorkoutDayExerciseResponseDto
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string? description { get; set; }

        public BodyPartDto bodyPartDto { get; set; }
    }
}
