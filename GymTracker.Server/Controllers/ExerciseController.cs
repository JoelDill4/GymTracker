using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseManager _exerciseManager;
        private readonly ILogger<ExerciseController> _logger;

        public ExerciseController(IExerciseManager exerciseManager, ILogger<ExerciseController> logger)
        {
            _exerciseManager = exerciseManager;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDto>>> GetExercises()
        {
            var exercises = await _exerciseManager.GetExercisesAsync();
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseResponseDto>> GetExercise(Guid id)
        {
            var exercise = await _exerciseManager.GetExerciseAsync(id);
            
            if (exercise == null)
            {
                return NotFound($"Exercise with ID {id} not found");
            }

            return Ok(exercise);
        }

        [HttpGet("bodypart/{bodyPart}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDto>>> GetExercisesByBodyPart(Guid bodyPart)
        {
            var exercises = await _exerciseManager.GetExercisesByBodyPartAsync(bodyPart);
            return Ok(exercises);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ExerciseResponseDto>> CreateExercise([FromBody] ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdExercise = await _exerciseManager.CreateExerciseAsync(exerciseDto);
                return CreatedAtAction(nameof(GetExercise), new { id = createdExercise.id }, createdExercise);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise");
                return StatusCode(500, "An error occurred while creating the exercise");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseResponseDto>> UpdateExercise(Guid id, [FromBody] ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedExercise = await _exerciseManager.UpdateExerciseAsync(id, exerciseDto);
                return Ok(updatedExercise);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exercise");
                return StatusCode(500, "An error occurred while updating the exercise");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExercise(Guid id)
        {
            try
            {
                await _exerciseManager.DeleteExerciseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Exercise with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exercise");
                return StatusCode(500, "An error occurred while deleting the exercise");
            }
        }
    }
}
