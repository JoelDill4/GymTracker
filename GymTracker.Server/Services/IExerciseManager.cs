using GymTracker.Server.Dtos;

namespace GymTracker.Server.Services
{
    public interface IExerciseManager
    {
        public List<Exercise> GetExercises();

        public Exercise? GetExercise(Guid id);

        public Exercise CreateOrEditExercise(ExerciseDto exercise);

        public void DeleteExercise(Guid id);
    }
}
