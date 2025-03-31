using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class WorkoutDayManager : IWorkoutDayManager
    {
        private readonly GymTrackerContext _context;

        public WorkoutDayManager(GymTrackerContext context)
        {
            _context = context;
        }

        /*public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysAsync()
        {
            return await _context.WorkoutDay
                .Where(wd => !wd.IsDeleted)
                .Include(wd => wd.Routine)
                .Include(wd => wd.WorkoutDayExercises)
                    .ThenInclude(wde => wde.Exercise)
                        .ThenInclude(e => e.ExerciseBodyParts)
                            .ThenInclude(eb => eb.BodyPart)
                .Select(wd => new WorkoutDayResponseDto
                {
                    Id = wd.Id,
                    Name = wd.Name,
                    CreatedAt = wd.CreatedAt,
                    UpdatedAt = wd.UpdatedAt,
                    RoutineId = wd.RoutineId,
                    RoutineName = wd.Routine.Name,
                    Exercises = wd.WorkoutDayExercises
                        .Select(wde => new WorkoutDayExerciseResponseDto
                        {
                            Id = wde.Id,
                            ExerciseId = wde.fk_exercise,
                            ExerciseName = wde.Exercise.Name,
                            BodyParts = wde.Exercise.ExerciseBodyParts
                                .Select(eb => new BodyPartDto
                                {
                                    Id = eb.BodyPart.Id,
                                    Name = eb.BodyPart.Name
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<WorkoutDayResponseDto?> GetWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.Routine)
                .Include(wd => wd.WorkoutDayExercises)
                    .ThenInclude(wde => wde.Exercise)
                        .ThenInclude(e => e.ExerciseBodyParts)
                            .ThenInclude(eb => eb.BodyPart)
                .FirstOrDefaultAsync(wd => wd.Id == id && !wd.IsDeleted);

            if (workoutDay == null)
                return null;

            return new WorkoutDayResponseDto
            {
                Id = workoutDay.Id,
                Name = workoutDay.Name,
                CreatedAt = workoutDay.CreatedAt,
                UpdatedAt = workoutDay.UpdatedAt,
                RoutineId = workoutDay.RoutineId,
                RoutineName = workoutDay.Routine.Name,
                Exercises = workoutDay.WorkoutDayExercises
                    .Select(wde => new WorkoutDayExerciseResponseDto
                    {
                        Id = wde.Id,
                        ExerciseId = wde.ExerciseId,
                        ExerciseName = wde.Exercise.Name,
                        Sets = wde.Sets,
                        Reps = wde.Reps,
                        Weight = wde.Weight,
                        BodyParts = wde.Exercise.ExerciseBodyParts
                            .Select(eb => new BodyPartDto
                            {
                                Id = eb.BodyPart.Id,
                                Name = eb.BodyPart.Name
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }

        public async Task<WorkoutDayResponseDto> CreateWorkoutDayAsync(WorkoutDayDto workoutDayDto)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.Id == workoutDayDto.RoutineId && !r.IsDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {workoutDayDto.RoutineId} not found");

            var workoutDay = new WorkoutDay
            {
                Id = Guid.NewGuid(),
                Name = workoutDayDto.Name,
                RoutineId = workoutDayDto.RoutineId,
                CreatedAt = DateTime.UtcNow
            };

            // Add exercises
            foreach (var exercise in workoutDayDto.Exercises)
            {
                var exerciseEntity = await _context.Exercise
                    .FirstOrDefaultAsync(e => e.Id == exercise.ExerciseId && !e.IsDeleted);

                if (exerciseEntity == null)
                    throw new KeyNotFoundException($"Exercise with ID {exercise.ExerciseId} not found");

                workoutDay.WorkoutDayExercises.Add(new WorkoutDayExercise
                {
                    WorkoutDayId = workoutDay.Id,
                    ExerciseId = exercise.ExerciseId,
                    Sets = exercise.Sets,
                    Reps = exercise.Reps,
                    Weight = exercise.Weight
                });
            }

            _context.WorkoutDay.Add(workoutDay);
            await _context.SaveChangesAsync();

            return await GetWorkoutDayAsync(workoutDay.Id);
        }

        public async Task<WorkoutDayResponseDto> UpdateWorkoutDayAsync(Guid id, WorkoutDayDto workoutDayDto)
        {
            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutDayExercises)
                .FirstOrDefaultAsync(wd => wd.Id == id && !wd.IsDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {id} not found");

            var routine = await _context.Routines
                .FirstOrDefaultAsync(r => r.Id == workoutDayDto.RoutineId && !r.IsDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {workoutDayDto.RoutineId} not found");

            workoutDay.Name = workoutDayDto.Name;
            workoutDay.RoutineId = workoutDayDto.RoutineId;
            workoutDay.UpdatedAt = DateTime.UtcNow;

            // Update exercises
            _context.WorkoutDayExercises.RemoveRange(workoutDay.WorkoutDayExercises);
            foreach (var exercise in workoutDayDto.Exercises)
            {
                var exerciseEntity = await _context.Exercises
                    .FirstOrDefaultAsync(e => e.Id == exercise.ExerciseId && !e.IsDeleted);

                if (exerciseEntity == null)
                    throw new KeyNotFoundException($"Exercise with ID {exercise.ExerciseId} not found");

                workoutDay.WorkoutDayExercises.Add(new WorkoutDayExercise
                {
                    WorkoutDayId = workoutDay.Id,
                    ExerciseId = exercise.ExerciseId,
                    Sets = exercise.Sets,
                    Reps = exercise.Reps,
                    Weight = exercise.Weight
                });
            }

            await _context.SaveChangesAsync();
            return await GetWorkoutDayAsync(workoutDay.Id);
        }

        public async Task<bool> DeleteWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDays
                .FirstOrDefaultAsync(wd => wd.Id == id && !wd.IsDeleted);

            if (workoutDay == null)
                return false;

            workoutDay.IsDeleted = true;
            workoutDay.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysByRoutineAsync(Guid routineId)
        {
            return await _context.WorkoutDays
                .Where(wd => !wd.IsDeleted && wd.RoutineId == routineId)
                .Include(wd => wd.Routine)
                .Include(wd => wd.WorkoutDayExercises)
                    .ThenInclude(wde => wde.Exercise)
                        .ThenInclude(e => e.ExerciseBodyParts)
                            .ThenInclude(eb => eb.BodyPart)
                .Select(wd => new WorkoutDayResponseDto
                {
                    Id = wd.Id,
                    Name = wd.Name,
                    CreatedAt = wd.CreatedAt,
                    UpdatedAt = wd.UpdatedAt,
                    RoutineId = wd.RoutineId,
                    RoutineName = wd.Routine.Name,
                    Exercises = wd.WorkoutDayExercises
                        .Select(wde => new WorkoutDayExerciseResponseDto
                        {
                            Id = wde.Id,
                            ExerciseId = wde.ExerciseId,
                            ExerciseName = wde.Exercise.Name,
                            Sets = wde.Sets,
                            Reps = wde.Reps,
                            Weight = wde.Weight,
                            BodyParts = wde.Exercise.ExerciseBodyParts
                                .Select(eb => new BodyPartDto
                                {
                                    Id = eb.BodyPart.Id,
                                    Name = eb.BodyPart.Name
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();
        }*/
    }
}
