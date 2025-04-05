﻿using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.Routine;
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

        public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysAsync()
        {
            return await _context.WorkoutDay
                .Where(wd => !wd.isDeleted)
                .Include(wd => wd.routine)
                .Select(wd => new WorkoutDayResponseDto
                {
                    id = wd.id,
                    name = wd.name,
                    description = wd.description,
                    routine = new RoutineResponseDto
                    {
                        id = wd.routine.id,
                        name = wd.routine.name,
                        description = wd.routine.description,
                        createdAt = wd.routine.createdAt,
                        updatedAt = wd.routine.updatedAt
                    },
                    createdAt = wd.createdAt,
                    updatedAt = wd.updatedAt
                })
                .ToListAsync();
        }

        public async Task<WorkoutDayResponseDto?> GetWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.routine)
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                return null;

            return new WorkoutDayResponseDto
            {
                id = workoutDay.id,
                name = workoutDay.name,
                routine = new RoutineResponseDto
                {
                    id = workoutDay.routine.id,
                    name = workoutDay.routine.name,
                    description = workoutDay.routine.description,
                    createdAt = workoutDay.routine.createdAt,
                    updatedAt = workoutDay.routine.updatedAt
                },
                createdAt = workoutDay.createdAt,
                updatedAt = workoutDay.updatedAt
            };
        }

        public async Task<IEnumerable<WorkoutDayResponseDto>> GetWorkoutDaysByRoutineAsync(Guid routineId)
        {
            return await _context.WorkoutDay
                .Where(wd => !wd.isDeleted && wd.routineId == routineId)
                .Include(wd => wd.routine)
                .Select(wd => new WorkoutDayResponseDto
                {
                    id = wd.id,
                    name = wd.name,
                    description = wd.description,
                    createdAt = wd.createdAt,
                    updatedAt = wd.updatedAt
                })
                .ToListAsync();
        }


        public async Task<WorkoutDayResponseDto> CreateWorkoutDayAsync(WorkoutDayDto workoutDayDto)
        {
            var routine = await _context.Routine
                .FirstOrDefaultAsync(r => r.id == workoutDayDto.routineId && !r.isDeleted);

            if (routine == null)
                throw new KeyNotFoundException($"Routine with ID {workoutDayDto.routineId} not found");

            var workoutDay = new WorkoutDay
            {
                id = Guid.NewGuid(),
                name = workoutDayDto.name,
                routineId = workoutDayDto.routineId,
                createdAt = DateTime.UtcNow
            };

            /* Add exercises
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
            }*/

            _context.WorkoutDay.Add(workoutDay);
            await _context.SaveChangesAsync();

            return await GetWorkoutDayAsync(workoutDay.id);
        }

        public async Task<WorkoutDayResponseDto> UpdateWorkoutDayAsync(Guid id, WorkoutDayDto workoutDayDto)
        {
            var workoutDay = await _context.WorkoutDay
                .Include(wd => wd.routine)
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                throw new KeyNotFoundException($"Workout day with ID {id} not found");

            workoutDay.name = workoutDayDto.name;
            workoutDay.description = workoutDayDto.description;
            workoutDay.routineId = workoutDayDto.routineId;
            workoutDay.updatedAt = DateTime.UtcNow;

            /* Update exercises
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
            }*/

            await _context.SaveChangesAsync();
            return await GetWorkoutDayAsync(workoutDay.id);
        }

        public async Task<bool> DeleteWorkoutDayAsync(Guid id)
        {
            var workoutDay = await _context.WorkoutDay
                .FirstOrDefaultAsync(wd => wd.id == id && !wd.isDeleted);

            if (workoutDay == null)
                return false;

            workoutDay.isDeleted = true;
            workoutDay.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
