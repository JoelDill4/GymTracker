using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.BodyPart;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.ExerciseSet;
using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Dtos.WorkoutDay;
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

        public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsAsync()
        {
            var workouts = await _context.Workout
                .Where(w => !w.isDeleted)
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(es => es.exercise)
                        .ThenInclude(e => e.bodyPart)
                .OrderByDescending(w => w.workoutDate)
                .ToListAsync();

            return workouts.Select(w => MapToWorkoutResponseDto(w));
        }

        public async Task<WorkoutResponseDto?> GetWorkoutAsync(Guid id)
        {
            var workout = await _context.Workout
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(es => es.exercise)
                        .ThenInclude(e => e.bodyPart)
                .FirstOrDefaultAsync(w => w.id == id && !w.isDeleted);

            return workout == null ? null : MapToWorkoutResponseDto(workout);
        }

        public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            var workoutsQuery = _context.Workout
                                    .Where(w => !w.isDeleted);

            if (startDate.HasValue)
            {
                workoutsQuery = workoutsQuery.Where(w => w.workoutDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                workoutsQuery = workoutsQuery.Where(w => w.workoutDate <= endDate.Value);
            }

            var workouts = await workoutsQuery
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(es => es.exercise)
                        .ThenInclude(e => e.bodyPart)
                .OrderByDescending(w => w.workoutDate)
                .ToListAsync();

            return workouts.Select(w => MapToWorkoutResponseDto(w));
        }

        public async Task<IEnumerable<WorkoutResponseDto>> GetWorkoutsByWorkoutDayAsync(Guid workoutDayId)
        {
            var workouts = await _context.Workout
                .Where(w => !w.isDeleted && w.fk_workoutDay == workoutDayId)
                .Include(w => w.workoutDay)
                    .ThenInclude(wd => wd.routine)
                .Include(w => w.exerciseSets)
                    .ThenInclude(es => es.exercise)
                        .ThenInclude(e => e.bodyPart)
                .OrderByDescending(w => w.workoutDate)
                .ToListAsync();

            return workouts.Select(w => MapToWorkoutResponseDto(w));
        }

        public async Task<WorkoutResponseDto> CreateWorkoutAsync(WorkoutDto workoutDto)
        {
            var workout = new Workout
            {
                workoutDate = workoutDto.workoutDate,
                observations = workoutDto.observations,
                fk_workoutDay = workoutDto.workoutDayId
            };

            _context.Workout.Add(workout);
            await _context.SaveChangesAsync();

            return await GetWorkoutAsync(workout.id) ?? throw new Exception("Failed to create workout");
        }

        public async Task<WorkoutResponseDto> UpdateWorkoutAsync(Guid id, WorkoutDto workoutDto)
        {
            var workout = await _context.Workout.FindAsync(id);
            if (workout == null || workout.isDeleted)
            {
                throw new KeyNotFoundException($"Workout with ID {id} not found");
            }

            workout.workoutDate = workoutDto.workoutDate;
            workout.observations = workoutDto.observations;
            workout.fk_workoutDay = workoutDto.workoutDayId;
            workout.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetWorkoutAsync(id) ?? throw new Exception("Failed to update workout");
        }

        public async Task<bool> DeleteWorkoutAsync(Guid id)
        {
            var workout = await _context.Workout.FindAsync(id);
            if (workout == null || workout.isDeleted)
            {
                return false;
            }

            workout.isDeleted = true;
            workout.updatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public List<ExerciseSetDto> GetExerciseSetsFromWorkout(Guid workoutId)
        {
            var exerciseSets = _context.ExerciseSet
                .Where(es => es.fk_workout == workoutId && !es.isDeleted)
                .OrderBy(es => es.order)
                .ToList();

            return exerciseSets.Select(es => new ExerciseSetDto
            {
                order = es.order,
                weight = es.weight,
                reps = es.reps,
                exerciseId = es.fk_exercise
            }).ToList();
        }

        public void AssignExerciseSetsOfExerciseToWorkout(Guid workoutId, Guid exerciseId, List<ExerciseSetDto> exerciseSets)
        {
            var workout = _context.Workout.Find(workoutId);
            if (workout == null || workout.isDeleted)
            {
                throw new KeyNotFoundException($"Workout with ID {workoutId} not found");
            }

            var exercise = _context.Exercise.Find(exerciseId);
            if (exercise == null || exercise.isDeleted)
            {
                throw new KeyNotFoundException($"Exercise with ID {exerciseId} not found");
            }

            // Remove existing exercise sets for this exercise
            var existingSets = _context.ExerciseSet
                .Where(es => es.fk_workout == workoutId && es.fk_exercise == exerciseId && !es.isDeleted)
                .ToList();

            foreach (var set in existingSets)
            {
                set.isDeleted = true;
                set.updatedAt = DateTime.UtcNow;
            }

            // Add new exercise sets
            foreach (var setDto in exerciseSets)
            {
                var exerciseSet = new ExerciseSet
                {
                    order = setDto.order,
                    weight = setDto.weight,
                    reps = setDto.reps,
                    fk_workout = workoutId,
                    fk_exercise = exerciseId
                };

                _context.ExerciseSet.Add(exerciseSet);
            }

            _context.SaveChanges();
        }

        public void AddExerciseSetToWorkout(Guid workoutId, ExerciseSetDto exerciseSetDto)
        {
            var workout = _context.Workout.Find(workoutId);
            if (workout == null || workout.isDeleted)
            {
                throw new KeyNotFoundException($"Workout with ID {workoutId} not found");
            }

            var exercise = _context.Exercise.Find(exerciseSetDto.exerciseId);
            if (exercise == null || exercise.isDeleted)
            {
                throw new KeyNotFoundException($"Exercise with ID {exerciseSetDto.exerciseId} not found");
            }

            var exerciseSet = new ExerciseSet
            {
                order = exerciseSetDto.order,
                weight = exerciseSetDto.weight,
                reps = exerciseSetDto.reps,
                fk_workout = workoutId,
                fk_exercise = exerciseSetDto.exerciseId
            };

            _context.ExerciseSet.Add(exerciseSet);
            _context.SaveChanges();
        }

        public void RemoveExerciseSetFromWorkout(Guid workoutId, Guid exerciseSetId)
        {
            var exerciseSet = _context.ExerciseSet
                .FirstOrDefault(es => es.id == exerciseSetId && es.fk_workout == workoutId && !es.isDeleted);

            if (exerciseSet == null)
            {
                throw new KeyNotFoundException($"Exercise set with ID {exerciseSetId} not found in workout {workoutId}");
            }

            exerciseSet.isDeleted = true;
            exerciseSet.updatedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }

        private static WorkoutResponseDto MapToWorkoutResponseDto(Workout workout)
        {
            return new WorkoutResponseDto
            {
                id = workout.id,
                workoutDate = workout.workoutDate,
                observations = workout.observations,
                workoutDay = workout.workoutDay != null ? new WorkoutDayResponseDto
                {
                    id = workout.workoutDay.id,
                    name = workout.workoutDay.name,
                    description = workout.workoutDay.description,
                    routine = workout.workoutDay.routine != null ? new RoutineResponseDto
                    {
                        id = workout.workoutDay.routine.id,
                        name = workout.workoutDay.routine.name,
                        description = workout.workoutDay.routine.description,
                        createdAt = workout.workoutDay.routine.createdAt,
                        updatedAt = workout.workoutDay.routine.updatedAt
                    } : null,
                    createdAt = workout.workoutDay.createdAt,
                    updatedAt = workout.workoutDay.updatedAt
                } : null,
                exerciseSets = workout.exerciseSets
                    .Where(es => !es.isDeleted)
                    .OrderBy(es => es.order)
                    .Select(es => new ExerciseSetResponseDto
                    {
                        id = es.id,
                        order = es.order,
                        weight = es.weight,
                        reps = es.reps,
                        exercise = new ExerciseResponseDto
                        {
                            id = es.exercise.id,
                            name = es.exercise.name,
                            description = es.exercise.description,
                            bodyPart = new BodyPartDto
                            {
                                id = es.exercise.bodyPart.id,
                                name = es.exercise.bodyPart.name
                            },
                            createdAt = es.exercise.createdAt,
                            updatedAt = es.exercise.updatedAt
                        },
                        createdAt = es.createdAt,
                        updatedAt = es.updatedAt
                    }).ToList(),
                createdAt = workout.createdAt,
                updatedAt = workout.updatedAt
            };
        }
    }
}
