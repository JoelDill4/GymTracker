using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    /// <summary>
    /// Controller for managing exercises
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseManager _exerciseManager;
        private readonly ILogger<ExerciseController> _logger;

        /// <summary>
        /// Initializes a new instance of ExerciseController
        /// </summary>
        /// <param name="exerciseManager">The exercise manager service</param>
        /// <param name="logger">The logger service</param>
        public ExerciseController(IExerciseManager exerciseManager, ILogger<ExerciseController> logger)
        {
            _exerciseManager = exerciseManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets all non-deleted exercises
        /// </summary>
        /// <returns>A collection of exercises</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDto>>> GetExercises()
        {
            var exercises = await _exerciseManager.GetExercisesAsync();
            return Ok(exercises);
        }

        /// <summary>
        /// Gets a specific exercise by ID
        /// </summary>
        /// <param name="id">The ID of the exercise to retrieve</param>
        /// <returns>The exercise if found</returns>
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

        /// <summary>
        /// Gets exercises by name
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>A collection of exercises matching the name</returns>
        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExerciseResponseDto>> GetExerciseByName(string name)
        {
            var exercise = await _exerciseManager.GetExercisesByNameAsync(name);
            return Ok(exercise);
        }

        /// <summary>
        /// Gets exercises by body part
        /// </summary>
        /// <param name="bodyPart">The ID of the body part to filter by</param>
        /// <returns>A collection of exercises for the specified body part</returns>
        [HttpGet("bodypart/{bodyPart}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDto>>> GetExercisesByBodyPart(Guid bodyPart)
        {
            var exercises = await _exerciseManager.GetExercisesByBodyPartAsync(bodyPart);
            return Ok(exercises);
        }

        /// <summary>
        /// Creates a new exercise
        /// </summary>
        /// <param name="exerciseDto">The exercise data to create</param>
        /// <returns>The created exercise</returns>
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

        /// <summary>
        /// Updates an existing exercise
        /// </summary>
        /// <param name="id">The ID of the exercise to update</param>
        /// <param name="exerciseDto">The updated exercise data</param>
        /// <returns>The updated exercise</returns>
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

        /// <summary>
        /// Deletes an exercise
        /// </summary>
        /// <param name="id">The ID of the exercise to delete</param>
        /// <returns>No content if successful</returns>
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
