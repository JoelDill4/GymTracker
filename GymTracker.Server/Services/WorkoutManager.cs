using GymTracker.Server.Dtos;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class WorkoutManager : IWorkoutManager
    {
        private readonly AppDbContext context;

        private readonly IWorkoutDayManager workoutDayManager;

        private readonly IExerciseManager exerciseManager;

        public WorkoutManager(AppDbContext context, IWorkoutDayManager workoutDayManager, IExerciseManager exerciseManager) 
        {
            this.context = context;
            this.workoutDayManager = workoutDayManager;
            this.exerciseManager = exerciseManager;
        }

        public List<Workout> GetWorkouts()
        {
            List<Workout> workouts = context.Workout
                                            .ToList();

            return workouts;
        }

        public Workout? GetWorkout(Guid workoutId)
        {
            Workout? workout = context.Workout
                                      .FirstOrDefault(r => r.Id == workoutId);

            return workout;
        }

        public Workout CreateOrEditWorkout(WorkoutDto workoutDto)
        {
            WorkoutDay? workoutDay = workoutDayManager.GetWorkoutDay(workoutDto.fk_workoutday);

            if (workoutDay == null)
            {
                throw new ArgumentException($"No workoutday found with ID: {workoutDto.Id}");
            }

            if (workoutDto.Id != null)
            {
                var existingWorkout = context.Workout.FirstOrDefault(r => r.Id == workoutDto.Id);

                if (existingWorkout == null)
                {
                    throw new ArgumentException($"No workout found with ID: {workoutDto.Id}");
                }

                existingWorkout.Date = workoutDto.Date;
                existingWorkout.Observations = workoutDto.Observations;
                existingWorkout.fk_workoutday = workoutDto.fk_workoutday;
   
                context.Workout.Update(existingWorkout);
                context.SaveChanges();
                return existingWorkout;
            }

            Workout workout = new Workout(workoutDto.Date, workoutDto.Observations, workoutDto.fk_workoutday, workoutDay);

            context.Workout.Add(workout);
            context.SaveChanges();
            return workout;
        }

        public void DeleteWorkout(Guid id)
        {
            Workout? workout = this.GetWorkout(id);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{id}\" does not exist.");
            }

            context.Workout.Remove(workout);
            context.SaveChanges();
        }

        public List<Exercise> GetExercisesFromWorkout(Guid workoutId)
        {
            var workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            return workout.Exercises;
        }

        public void AddExerciseToWorkout(Guid workoutId, Guid exerciseId)
        {
            Workout? workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseId}\" does not exist.");
            }

            if (workout.Exercises.Contains(exercise))
            {
                throw new ArgumentException($"The exercise is already part of the workout");
            }

            workout.Exercises.Add(exercise);

            context.Update(workout);
            context.SaveChanges();
        }

        public void RemoveExerciseFromWorkout(Guid workoutId, Guid exerciseId)
        {
            Workout? workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseId}\" does not exist.");
            }

            if (!workout.Exercises.Contains(exercise))
            {
                throw new ArgumentException($"The exercise is not part of the workout");
            }

            workout.Exercises.Remove(exercise);

            context.Update(workout);
            context.SaveChanges();
        }

        public List<ExerciseExecutionDto> GetExercisesExecutionsFromWorkout(Guid workoutId)
        {
            var workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            List<ExerciseExecutionDto> exercisesExecutions = context.ExerciseExecution
                                             .Where(x => x.fk_workout == workoutId)
                                             .GroupBy(x => new { x.fk_exercise, x.Exercise.Name })
                                             .Select(g => new ExerciseExecutionDto
                                             {
                                                 ExerciseId = g.Key.fk_exercise,
                                                 ExerciseName = g.Key.Name,
                                                 Weights = g.Select(x => x.Weight).ToList(),
                                                 Reps = g.Select(x => x.Reps).ToList()
                                             })
                                             .ToList();

            return exercisesExecutions;
        }

        public void AddExerciseExecutionToWorkout(Guid workoutId, ExerciseExecutionDto exerciseExecutionDto)
        {
            Workout? workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseExecutionDto.ExerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseExecutionDto.ExerciseId}\" does not exist.");
            }

            if (!workout.Exercises.Contains(exercise))
            {
                throw new ArgumentException($"The exercise is not part of the workout");
            }

            if (exerciseExecutionDto.Weights.Count() != exerciseExecutionDto.Reps.Count())
            {
                throw new ArgumentException($"The number of weights and reps must be the same");
            }

            List<ExerciseExecution> exerciseExecutions = new List<ExerciseExecution>();

            for (int i = 0; i < exerciseExecutionDto.Weights.Count(); ++i)
            {
                ExerciseExecution exerciseExecution = new ExerciseExecution(0, exerciseExecutionDto.Weights[i], exerciseExecutionDto.Reps[i], workout, exercise);

                exerciseExecutions.Add(exerciseExecution);
            }

            context.Add(exerciseExecutions);
            context.SaveChanges();
        }

        public void RemoveExerciseExecutionFromWorkout(Guid workoutId, Guid exerciseId)
        {
            Workout? workout = this.GetWorkout(workoutId);

            if (workout == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseId}\" does not exist.");
            }

            if (!workout.Exercises.Contains(exercise))
            {
                throw new ArgumentException($"The exercise is not part of the workout");
            }

            List<ExerciseExecution> exercisesExecutions = context.ExerciseExecution
                                                                 .Where(x => x.fk_workout == workoutId && x.fk_exercise == exerciseId)
                                                                 .ToList();
            
            if (exercisesExecutions.Count() == 0)
            {
                throw new ArgumentException($"The exercise has not been executed in this workout");
            }
            
            context.Remove(exercisesExecutions);
            context.SaveChanges();
        }
    }
}
