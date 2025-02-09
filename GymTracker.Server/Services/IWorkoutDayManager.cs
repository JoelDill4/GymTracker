using GymTracker.Server.Dtos;

namespace GymTracker.Server.Services
{
    public interface IWorkoutDayManager
    {
        public List<WorkoutDay> GetWorkoutDays();

        public WorkoutDay? GetWorkoutDay(Guid id);

        public WorkoutDay CreateOrEditWorkoutDay(WorkoutDayDto workoutDay);

        public void DeleteWorkoutDay(Guid id);

        public List<Exercise> GetExercisesFromWorkoutDay(Guid workoutDayId);

        public void AddExerciseToWorkoutDay(Guid workoutDayId, Guid exerciseId);

        public void RemoveExerciseFromWorkoutDay(Guid workoutDayId, Guid exerciseId);
    }
}
