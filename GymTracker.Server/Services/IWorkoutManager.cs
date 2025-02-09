using GymTracker.Server.Dtos;

namespace GymTracker.Server.Services
{
    public interface IWorkoutManager
    {
        public List<Workout> GetWorkouts();

        public Workout? GetWorkout(Guid id);

        public Workout CreateOrEditWorkout(WorkoutDto workout);

        public void DeleteWorkout(Guid id);

        public List<Exercise> GetExercisesFromWorkout(Guid workoutId);

        public void AddExerciseToWorkout(Guid workoutId, Guid exerciseId);

        public void RemoveExerciseFromWorkout(Guid workoutId, Guid exerciseId);

        public List<ExerciseExecutionDto> GetExercisesExecutionsFromWorkout(Guid workoutId);

        public void AddExerciseExecutionToWorkout(Guid workoutId, ExerciseExecutionDto exercise);

        public void RemoveExerciseExecutionFromWorkout(Guid workoutId, Guid exerciseId);
    }
}
