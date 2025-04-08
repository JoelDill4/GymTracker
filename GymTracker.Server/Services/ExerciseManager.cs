using GymTracker.Server.DatabaseConnection;
using GymTracker.Server.Dtos.BodyPart;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class ExerciseManager : IExerciseManager
    {
        private readonly GymTrackerContext _context;

        public ExerciseManager(GymTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExerciseResponseDto>> GetExercisesAsync()
        {
            var exercises = await _context.Exercise
                .Include(e => e.bodyPart)
                .Where(e => !e.isDeleted)
                .ToListAsync();

            return exercises.Select(e => new ExerciseResponseDto
            {
                id = e.id,
                name = e.name,
                description = e.description,
                createdAt = e.createdAt,
                updatedAt = e.updatedAt,
                bodyPart = new BodyPartDto
                {
                    id = e.bodyPart.id,
                    name = e.bodyPart.name
                }
            });
        }

        public async Task<IEnumerable<ExerciseResponseDto>> GetExercisesByNameAsync(string name)
        {
            var exercises = await _context.Exercise
                .Include(e => e.bodyPart)
                .Where(e => !e.isDeleted && e.name.Contains(name))
                .ToListAsync();

            return exercises.Select(e => new ExerciseResponseDto
            {
                id = e.id,
                name = e.name,
                description = e.description,
                createdAt = e.createdAt,
                updatedAt = e.updatedAt,
                bodyPart = new BodyPartDto
                {
                    id = e.bodyPart.id,
                    name = e.bodyPart.name
                }
            });
        }

        public async Task<ExerciseResponseDto?> GetExerciseAsync(Guid id)
        {
            var exercise = await _context.Exercise
                .Include(e => e.bodyPart)
                .FirstOrDefaultAsync(e => e.id == id && !e.isDeleted);

            if (exercise == null)
                return null;

            return new ExerciseResponseDto
            {
                id = exercise.id,
                name = exercise.name,
                description = exercise.description,
                createdAt = exercise.createdAt,
                updatedAt = exercise.updatedAt,
                bodyPart = new BodyPartDto
                {
                    id = exercise.bodyPart.id,
                    name = exercise.bodyPart.name
                }
            };
        }

        public async Task<ExerciseResponseDto> CreateExerciseAsync([FromBody] ExerciseDto exerciseDto)
        {
            var bodyPart = await _context.BodyPart.FindAsync(exerciseDto.fk_bodyPart);
            if (bodyPart == null)
                throw new KeyNotFoundException($"Body part with ID {exerciseDto.fk_bodyPart} not found");

            var exercise = new Exercise
            {
                id = Guid.NewGuid(),
                name = exerciseDto.name,
                description = exerciseDto.description,
                fk_bodyPart = exerciseDto.fk_bodyPart,
                createdAt = DateTime.UtcNow
            };

            _context.Exercise.Add(exercise);
            await _context.SaveChangesAsync();

            return await GetExerciseAsync(exercise.id);
        }

        public async Task<ExerciseResponseDto> UpdateExerciseAsync(Guid id, [FromBody] ExerciseDto exerciseDto)
        {
            var exercise = await _context.Exercise
                .FirstOrDefaultAsync(e => e.id == id && !e.isDeleted);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise with ID {id} not found");

            var bodyPart = await _context.BodyPart.FindAsync(exerciseDto.fk_bodyPart);
            if (bodyPart == null)
                throw new KeyNotFoundException($"Body part with ID {exerciseDto.fk_bodyPart} not found");

            exercise.name = exerciseDto.name;
            exercise.description = exerciseDto.description;
            exercise.fk_bodyPart = exerciseDto.fk_bodyPart;
            exercise.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetExerciseAsync(exercise.id);
        }

        public async Task DeleteExerciseAsync(Guid id)
        {
            var exercise = await _context.Exercise
                .FirstOrDefaultAsync(e => e.id == id && !e.isDeleted);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise with ID {id} not found");

            exercise.isDeleted = true;
            exercise.updatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExerciseResponseDto>> GetExercisesByBodyPartAsync(Guid bodyPartId)
        {
            var exercises = await _context.Exercise
                .Include(e => e.bodyPart)
                .Where(e => !e.isDeleted && e.fk_bodyPart == bodyPartId)
                .ToListAsync();

            return exercises.Select(e => new ExerciseResponseDto
            {
                id = e.id,
                name = e.name,
                description = e.description,
                createdAt = e.createdAt,
                updatedAt = e.updatedAt,
                bodyPart = new BodyPartDto
                {
                    id = e.bodyPart.id,
                    name = e.bodyPart.name
                }
            });
        }
    }
}
