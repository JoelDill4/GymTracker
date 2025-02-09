using GymTracker.Server.Dtos;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class WorkoutDayManager : IWorkoutDayManager
    {
        private readonly AppDbContext context;

        private readonly IRoutineManager routineManager;

        private readonly IExerciseManager exerciseManager;

        public WorkoutDayManager(AppDbContext context, IRoutineManager routineManager, IExerciseManager exerciseManager) 
        {
            this.context = context;
            this.routineManager = routineManager;
            this.exerciseManager = exerciseManager;
        }

        public List<WorkoutDay> GetWorkoutDays()
        {
            List<WorkoutDay> workoutDays = context.WorkoutDay
                                                  .Include(x => x.Routine)
                                                  .ToList();

            return workoutDays;
        }

        public WorkoutDay? GetWorkoutDay(Guid id)
        {
            WorkoutDay? workoutday = context.WorkoutDay
                                            .Include(x => x.Routine)
                                            .Include(x => x.WorkoutDaysExercises)
                                            .FirstOrDefault(r => r.Id == id);

            return workoutday;
        }

        public WorkoutDay CreateOrEditWorkoutDay(WorkoutDayDto workoutDayDto)
        {
            Routine? routine = routineManager.GetRoutine(workoutDayDto.fk_routine);

            if (routine == null)
            {
                throw new ArgumentException($"No routine found with ID: {workoutDayDto.fk_routine}");
            }

            if (workoutDayDto.Id != null)
            {
                var existingWorkoutDay = context.WorkoutDay.FirstOrDefault(r => r.Id == workoutDayDto.Id);

                if (existingWorkoutDay == null)
                {
                    throw new ArgumentException($"No workout found with ID: {workoutDayDto.Id}");
                }

                existingWorkoutDay.Name = workoutDayDto.Name;
                existingWorkoutDay.fk_routine = workoutDayDto.fk_routine;
                existingWorkoutDay.Routine = routine;
   
                context.WorkoutDay.Update(existingWorkoutDay);
                context.SaveChanges();
                return existingWorkoutDay;
            }

            WorkoutDay workoutday = new WorkoutDay(workoutDayDto.Name, routine);

            context.WorkoutDay.Add(workoutday);
            context.SaveChanges();
            return workoutday;
        }

        public void DeleteWorkoutDay(Guid id)
        {
            WorkoutDay? workoutday = this.GetWorkoutDay(id);

            if (workoutday == null)
            {
                throw new ArgumentException($"A workout with Id: \"{id}\" does not exist.");
            }

            context.WorkoutDay.Remove(workoutday);
            context.SaveChanges();
        }

        public List<Exercise> GetExercisesFromWorkoutDay(Guid workoutDayId)
        {
            var workoutDay = this.GetWorkoutDay(workoutDayId);

            if (workoutDay == null)
            {
                throw new ArgumentException($"A workout with Id: \"{workoutDayId}\" does not exist.");
            }

            List<Exercise> exercises = context.WorkoutDayExercise
                                              .Where(x => x.fk_workoutday == workoutDayId)
                                              .Select(x => x.Exercise)
                                              .ToList();
            return exercises;
        }

        public void AddExerciseToWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            WorkoutDay? workoutDay = this.GetWorkoutDay(workoutDayId);

            if (workoutDay == null)
            {
                throw new ArgumentException($"A workoutday with Id: \"{workoutDayId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseId}\" does not exist.");
            }

            WorkoutDayExercise? workoutDayExercise = context.WorkoutDayExercise
                                                    .Where(x => x.fk_exercise == exerciseId && 
                                                                x.fk_workoutday == workoutDayId)
                                                    .FirstOrDefault();

            if (workoutDayExercise != null)
            {
                throw new ArgumentException($"The exercise is already part of the workoutday");
            }

            workoutDayExercise = new WorkoutDayExercise(workoutDay, exercise);

            context.Add(workoutDayExercise);
            context.SaveChanges();
        }

        public void RemoveExerciseFromWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            WorkoutDay? workoutDay = this.GetWorkoutDay(workoutDayId);

            if (workoutDay == null)
            {
                throw new ArgumentException($"A workoutday with Id: \"{workoutDayId}\" does not exist.");
            }

            Exercise? exercise = exerciseManager.GetExercise(exerciseId);

            if (exercise == null)
            {
                throw new ArgumentException($"A exercise with Id: \"{exerciseId}\" does not exist.");
            }

            WorkoutDayExercise? workoutDayExercise = context.WorkoutDayExercise
                                                    .Where(x => x.fk_exercise == exerciseId &&
                                                                x.fk_workoutday == workoutDayId)
                                                    .FirstOrDefault();

            if (workoutDayExercise == null)
            {
                throw new ArgumentException($"The exercise is not part of the workoutday");
            }

            context.Remove(workoutDayExercise);
            context.SaveChanges();
        }
    }
}
