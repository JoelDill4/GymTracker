using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    /// <summary>
    /// Controller for managing workout days
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class WorkoutDayController : ControllerBase
    {
        private readonly IWorkoutDayManager _workoutDayManager;
        private readonly ILogger<WorkoutDayController> _logger;

        /// <summary>
        /// Initializes a new instance of WorkoutDayController
        /// </summary>
        /// <param name="workoutDayManager">The workout day manager service</param>
        /// <param name="logger">The logger service</param>
        public WorkoutDayController(IWorkoutDayManager workoutDayManager, ILogger<WorkoutDayController> logger)
        {
            _workoutDayManager = workoutDayManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets all workout days
        /// </summary>
        /// <returns>A collection of workout days</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutDayResponseDto>>> GetWorkoutDays()
        {
            var workoutDays = await _workoutDayManager.GetWorkoutDaysAsync();
            return Ok(workoutDays);
        }

        /// <summary>
        /// Gets a specific workout day by ID
        /// </summary>
        /// <param name="id">The ID of the workout day to retrieve</param>
        /// <returns>The workout day if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkoutDayResponseDto>> GetWorkoutDay(Guid id)
        {
            var workoutDay = await _workoutDayManager.GetWorkoutDayAsync(id);
            
            if (workoutDay == null)
            {
                return NotFound($"Workout day with ID {id} not found");
            }

            return Ok(workoutDay);
        }

        /// <summary>
        /// Creates a new workout day
        /// </summary>
        /// <param name="workoutDayDto">The workout day data to create</param>
        /// <returns>The created workout day</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkoutDayResponseDto>> CreateWorkoutDay([FromBody] WorkoutDayDto workoutDayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdWorkoutDay = await _workoutDayManager.CreateWorkoutDayAsync(workoutDayDto);
                return CreatedAtAction(nameof(GetWorkoutDay), new { id = createdWorkoutDay.id }, createdWorkoutDay);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout day");
                return StatusCode(500, "An error occurred while creating the workout day");
            }
        }

        /// <summary>
        /// Updates an existing workout day
        /// </summary>
        /// <param name="id">The ID of the workout day to update</param>
        /// <param name="workoutDayDto">The updated workout day data</param>
        /// <returns>The updated workout day</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkoutDayResponseDto>> UpdateWorkoutDay(Guid id, [FromBody] WorkoutDayDto workoutDayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedWorkoutDay = await _workoutDayManager.UpdateWorkoutDayAsync(id, workoutDayDto);
                return Ok(updatedWorkoutDay);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Workout day with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workout day");
                return StatusCode(500, "An error occurred while updating the workout day");
            }
        }

        /// <summary>
        /// Gets all exercises from a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <returns>A collection of exercises</returns>
        [HttpGet("exercises/{workoutDayId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDto>>> GetExercisesFromWorkoutDay(Guid workoutDayId)
        {
            try
            {
                var exercises = await _workoutDayManager.GetExercisesFromWorkoutDayAsync(workoutDayId);
                return Ok(exercises);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Workout day with ID {workoutDayId} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercises from workout day");
                return StatusCode(500, "An error occurred while getting exercises from the workout day");
            }
        }

        /// <summary>
        /// Sets the exercises of a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exercisesIds">The list of exercise IDs that will be part of the workout day</param>
        /// <returns>No content if successful</returns>
        [HttpPost("exercises/{workoutDayId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignExercisesOfWorkoutDay(Guid workoutDayId, [FromBody] List<Guid> exercisesIds)
        {
            try
            {
                await _workoutDayManager.AssignExercisesToWorkoutDayAsync(workoutDayId, exercisesIds);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning exercises to workout day");
                return StatusCode(500, "An error occurred while assigning the exercises to the workout day");
            }
        }

        /// <summary>
        /// Adds an exercise to a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exerciseId">The ID of the exercise to add</param>
        /// <returns>No content if successful</returns>
        [HttpPost("exercises/{workoutDayId}/{exerciseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddExerciseToWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            try
            {
                await _workoutDayManager.AddExerciseToWorkoutDayAsync(workoutDayId, exerciseId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding exercise to workout day");
                return StatusCode(500, "An error occurred while adding the exercise to the workout day");
            }
        }

        /// <summary>
        /// Removes an exercise from a workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <param name="exerciseId">The ID of the exercise to remove</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("exercises/{workoutDayId}/{exerciseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveExerciseFromWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            try
            {
                await _workoutDayManager.RemoveExerciseFromWorkoutDayAsync(workoutDayId, exerciseId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing exercise from workout day");
                return StatusCode(500, "An error occurred while removing the exercise from the workout day");
            }
        }

        /// <summary>
        /// Deletes a workout day
        /// </summary>
        /// <param name="id">The ID of the workout day to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWorkoutDay(Guid id)
        {
            var deleted = await _workoutDayManager.DeleteWorkoutDayAsync(id);
            
            if (!deleted)
            {
                return NotFound($"Workout day with ID {id} not found");
            }

            return NoContent();
        }
    }
}
