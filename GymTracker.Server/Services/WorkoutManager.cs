using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class WorkoutManager : IWorkoutManager
    {
        private readonly GymTrackerContext _context;

        public WorkoutManager(GymTrackerContext context)
        {
            _context = context;
        }

        /*public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsAsync()
        {
            return await _context.Workout
                .Where(w => !w.isDeleted)
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(ee => ee.exercise)
                .Select(w => new WorkoutResponseDto
                {
                    Id = w.id,
                    Date = w.workoutDate,
                    Observations = w.observations,
                    CreatedAt = w.createdAt,
                    UpdatedAt = w.updatedAt,
                    WorkoutDay = new WorkoutDayDto
                    {
                        Id = w.fk_workoutDay,
                        Name = w.workoutDay.name,
                        RoutineId = w.workoutDay.fk_routine,
                        RoutineName = w.workoutDay.routine.name
                    },
                    ExerciseExecutions = w.exerciseSets
                        .Select(ee => new ExerciseExecutionDto
                        {
                            Id = ee.id,
                            ExerciseName = ee.exercise.name,
                            Reps = ee.reps,
                            Weight = ee.weight,
                            Notes = ee.obs
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<WorkoutResponseDto?> GetWorkoutAsync(Guid id)
        {
            var workout = await _context.Workout
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(ee => ee.exercise)
                .FirstOrDefaultAsync(w => w.id == id && !w.isDeleted);

            if (workout == null)
                return null;

            return new WorkoutResponseDto
            {
                Id = workout.Id,
                Date = workout.Date,
                Observations = workout.Observations,
                CreatedAt = workout.CreatedAt,
                UpdatedAt = workout.UpdatedAt,
                WorkoutDay = new WorkoutDayDto
                {
                    Id = workout.WorkoutDay.Id,
                    Name = workout.WorkoutDay.Name,
                    RoutineId = workout.WorkoutDay.RoutineId,
                    RoutineName = workout.WorkoutDay.Routine.Name
                },
                ExerciseExecutions = workout.ExerciseExecutions
                    .Select(ee => new ExerciseExecutionDto
                    {
                        Id = ee.Id,
                        ExerciseName = ee.Exercise.Name,
                        Sets = ee.Sets,
                        Reps = ee.Reps,
                        Weight = ee.Weight,
                        Notes = ee.Notes
                    })
                    .ToList()
            };
        }

        public async Task<WorkoutResponseDto> CreateWorkoutAsync(WorkoutDto workoutDto)
        {
            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.Routine)
                .FirstOrDefaultAsync(wd => wd.Id == workoutDto.WorkoutDayId && !wd.IsDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {workoutDto.WorkoutDayId} not found");

            var workout = new Workout
            {
                Id = Guid.NewGuid(),
                Date = workoutDto.Date,
                Observations = workoutDto.Observations,
                WorkoutDayId = workoutDto.WorkoutDayId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return await GetWorkoutAsync(workout.Id);
        }

        public async Task<WorkoutResponseDto> UpdateWorkoutAsync(Guid id, WorkoutDto workoutDto)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutDay)
                    .ThenInclude(wd => wd.Routine)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (workout == null)
                throw new KeyNotFoundException($"Workout with ID {id} not found");

            var workoutDay = await _context.WorkoutDays
                .FirstOrDefaultAsync(wd => wd.Id == workoutDto.WorkoutDayId && !wd.IsDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {workoutDto.WorkoutDayId} not found");

            workout.Date = workoutDto.Date;
            workout.Observations = workoutDto.Observations;
            workout.WorkoutDayId = workoutDto.WorkoutDayId;
            workout.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetWorkoutAsync(workout.Id);
        }

        public async Task<bool> DeleteWorkoutAsync(Guid id)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (workout == null)
                return false;

            workout.IsDeleted = true;
            workout.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Workouts
                .Where(w => !w.IsDeleted && w.Date >= startDate && w.Date <= endDate)
                .Include(w => w.WorkoutDay)
                    .ThenInclude(wd => wd.Routine)
                .Include(w => w.ExerciseExecutions)
                    .ThenInclude(ee => ee.Exercise)
                .Select(w => new WorkoutResponseDto
                {
                    Id = w.Id,
                    Date = w.Date,
                    Observations = w.Observations,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt,
                    WorkoutDay = new WorkoutDayDto
                    {
                        Id = w.WorkoutDay.Id,
                        Name = w.WorkoutDay.Name,
                        RoutineId = w.WorkoutDay.RoutineId,
                        RoutineName = w.WorkoutDay.Routine.Name
                    },
                    ExerciseExecutions = w.ExerciseExecutions
                        .Select(ee => new ExerciseExecutionDto
                        {
                            Id = ee.Id,
                            ExerciseName = ee.Exercise.Name,
                            Sets = ee.Sets,
                            Reps = ee.Reps,
                            Weight = ee.Weight,
                            Notes = ee.Notes
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByWorkoutDayAsync(Guid workoutDayId)
        {
            return await _context.Workouts
                .Where(w => !w.IsDeleted && w.WorkoutDayId == workoutDayId)
                .Include(w => w.WorkoutDay)
                    .ThenInclude(wd => wd.Routine)
                .Include(w => w.ExerciseExecutions)
                    .ThenInclude(ee => ee.Exercise)
                .Select(w => new WorkoutResponseDto
                {
                    Id = w.Id,
                    Date = w.Date,
                    Observations = w.Observations,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt,
                    WorkoutDay = new WorkoutDayDto
                    {
                        Id = w.WorkoutDay.Id,
                        Name = w.WorkoutDay.Name,
                        RoutineId = w.WorkoutDay.RoutineId,
                        RoutineName = w.WorkoutDay.Routine.Name
                    },
                    ExerciseExecutions = w.ExerciseExecutions
                        .Select(ee => new ExerciseExecutionDto
                        {
                            Id = ee.Id,
                            ExerciseName = ee.Exercise.Name,
                            Sets = ee.Sets,
                            Reps = ee.Reps,
                            Weight = ee.Weight,
                            Notes = ee.Notes
                        })
                        .ToList()
                })
                .ToListAsync();
        }*/
    }
}
