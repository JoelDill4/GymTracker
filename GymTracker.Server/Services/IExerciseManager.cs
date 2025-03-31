using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Models;

namespace GymTracker.Server.Services
{
    public interface IExerciseManager
    {
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesAsync();
        Task<ExerciseResponseDto?> GetExerciseAsync(Guid id);
        Task<ExerciseResponseDto> CreateExerciseAsync(ExerciseDto exerciseDto);
        Task<ExerciseResponseDto> UpdateExerciseAsync(Guid id, ExerciseDto exerciseDto);
        Task DeleteExerciseAsync(Guid id);
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesByBodyPartAsync(Guid bodyPartId);
    }
}
