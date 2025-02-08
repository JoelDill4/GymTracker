using GymTracker.Server.Dtos;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class ExerciseManager : IExerciseManager
    {
        private readonly AppDbContext context;

        private readonly IRoutineManager routineManager;

        public ExerciseManager(AppDbContext context, IRoutineManager routineManager) 
        {
            this.context = context;
            this.routineManager = routineManager;
        }

        public List<Exercise> GetExercises()
        {
            List<Exercise> exercises = context.Exercise
                                                  .ToList();

            return exercises;
        }

        public Exercise? GetExercise(Guid id)
        {
            Exercise? exercise = context.Exercise
                                            .FirstOrDefault(r => r.Id == id);

            return exercise;
        }

        public Exercise CreateOrEditExercise(ExerciseDto ExerciseDto)
        {
            if (ExerciseDto.Id != null)
            {
                var existingExercise = context.Exercise.FirstOrDefault(r => r.Id == ExerciseDto.Id);

                if (existingExercise == null)
                {
                    throw new ArgumentException($"No workout found with ID: {ExerciseDto.Id}");
                }

                existingExercise.Name = ExerciseDto.Name;

                context.Exercise.Update(existingExercise);
                context.SaveChanges();
                return existingExercise;
            }

            Exercise exercise = new Exercise(ExerciseDto.Name);

            context.Exercise.Add(exercise);
            context.SaveChanges();
            return exercise;
        }

        public void DeleteExercise(Guid id)
        {
            Exercise? exercise = this.GetExercise(id);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{id}\" does not exist.");
            }

            context.Exercise.Remove(exercise);
            context.SaveChanges();
        }
    }
}
