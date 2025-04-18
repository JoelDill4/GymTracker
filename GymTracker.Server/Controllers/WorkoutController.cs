using GymTracker.Server.Dtos.ExerciseSet;
using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    /// <summary>
    /// Controller for managing workouts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutManager _workoutManager;
        private readonly ILogger<WorkoutController> _logger;

        /// <summary>
        /// Initializes a new instance of WorkoutController
        /// </summary>
        /// <param name="workoutManager">The workout manager service</param>
        /// <param name="logger">The logger service</param>
        public WorkoutController(IWorkoutManager workoutManager, ILogger<WorkoutController> logger)
        {
            _workoutManager = workoutManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets all non-deleted workouts
        /// </summary>
        /// <returns>A collection of workouts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkouts()
        {
            var workouts = await _workoutManager.GetWorkoutsAsync();
            return Ok(workouts);
        }

        /// <summary>
        /// Gets a specific workout by ID
        /// </summary>
        /// <param name="id">The ID of the workout to retrieve</param>
        /// <returns>The workout if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkoutResponseDto>> GetWorkout(Guid id)
        {
            var workout = await _workoutManager.GetWorkoutAsync(id);
            
            if (workout == null)
            {
                return NotFound($"Workout with ID {id} not found");
            }

            return Ok(workout);
        }

        /// <summary>
        /// Gets workouts within a specified date range
        /// </summary>
        /// <param name="startDate">The start date of the range (optional)</param>
        /// <param name="endDate">The end date of the range (optional)</param>
        /// <returns>A collection of workouts within the date range</returns>
        [HttpGet("daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkoutsByDateRange(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var workouts = await _workoutManager.GetWorkoutsByDateRangeAsync(startDate, endDate);
            return Ok(workouts);
        }

        /// <summary>
        /// Gets all workouts associated with a specific workout day
        /// </summary>
        /// <param name="workoutDayId">The ID of the workout day</param>
        /// <returns>A collection of workouts for the specified workout day</returns>
        [HttpGet("workoutday/{workoutDayId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkoutsByWorkoutDay(Guid workoutDayId)
        {
            var workouts = await _workoutManager.GetWorkoutsByWorkoutDayAsync(workoutDayId);
            return Ok(workouts);
        }

        /// <summary>
        /// Creates a new workout
        /// </summary>
        /// <param name="workoutDto">The workout data to create</param>
        /// <returns>The created workout</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkoutResponseDto>> CreateWorkout([FromBody] WorkoutDto workoutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdWorkout = await _workoutManager.CreateWorkoutAsync(workoutDto);
                return CreatedAtAction(nameof(GetWorkout), new { id = createdWorkout.id }, createdWorkout);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workout");
                return StatusCode(500, "An error occurred while creating the workout");
            }
        }

        /// <summary>
        /// Updates an existing workout
        /// </summary>
        /// <param name="id">The ID of the workout to update</param>
        /// <param name="workoutDto">The updated workout data</param>
        /// <returns>The updated workout</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkoutResponseDto>> UpdateWorkout(Guid id, [FromBody] WorkoutDto workoutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedWorkout = await _workoutManager.UpdateWorkoutAsync(id, workoutDto);
                return Ok(updatedWorkout);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Workout with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workout");
                return StatusCode(500, "An error occurred while updating the workout");
            }
        }

        /// <summary>
        /// Deletes a workout
        /// </summary>
        /// <param name="id">The ID of the workout to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWorkout(Guid id)
        {
            var deleted = await _workoutManager.DeleteWorkoutAsync(id);
            
            if (!deleted)
            {
                return NotFound($"Workout with ID {id} not found");
            }

            return NoContent();
        }

        /// <summary>
        /// Gets all exercise sets from a workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <returns>A collection of exercise sets</returns>
        [HttpGet("getExerciseSets/{workoutId}")]
        public ActionResult<IEnumerable<ExerciseSetDto>> GetExerciseSetsFromWorkout(Guid workoutId)
        {
            try
            {
                var exerciseSets = _workoutManager.GetExerciseSetsFromWorkout(workoutId);
                return Ok(exerciseSets);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds an exercise set to a workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <param name="exerciseSetDto">The exercise set data to add</param>
        /// <returns>Ok if successful</returns>
        [HttpPost("addExerciseSet/{workoutId}")]
        public IActionResult AddExerciseSetToWorkout(Guid workoutId, [FromBody] ExerciseSetDto exerciseSetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _workoutManager.AddExerciseSetToWorkout(workoutId, exerciseSetDto);
                return Ok("The exercise set has been added to the workout");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding exercise set to workout");
                return StatusCode(500, "An error occurred while adding the exercise set to the workout");
            }
        }

        /// <summary>
        /// Removes an exercise set from a workout
        /// </summary>
        /// <param name="workoutId">The ID of the workout</param>
        /// <param name="exerciseSetId">The ID of the exercise set to remove</param>
        /// <returns>Ok if successful</returns>
        [HttpPost("removeExerciseSet/{workoutId}/{exerciseSetId}")]
        public IActionResult RemoveExerciseSetFromWorkout(Guid workoutId, Guid exerciseSetId)
        {
            try
            {
                _workoutManager.RemoveExerciseSetFromWorkout(workoutId, exerciseSetId);
                return Ok("The exercise set has been removed from the workout");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing exercise set from workout");
                return StatusCode(500, "An error occurred while removing the exercise set from the workout");
            }
        }
    }
}
