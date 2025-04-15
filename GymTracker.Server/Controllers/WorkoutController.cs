using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutManager _workoutManager;
        private readonly ILogger<WorkoutController> _logger;

        public WorkoutController(IWorkoutManager workoutManager, ILogger<WorkoutController> logger)
        {
            _workoutManager = workoutManager;
            _logger = logger;
        }

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkouts()
        {
            var workouts = await _workoutManager.GetWorkoutsAsync();
            return Ok(workouts);
        }

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

        [HttpGet("daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkoutsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var workouts = await _workoutManager.GetWorkoutsByDateRangeAsync(startDate, endDate);
            return Ok(workouts);
        }

        [HttpGet("workoutday/{workoutDayId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutResponseDto>>> GetWorkoutsByWorkoutDay(Guid workoutDayId)
        {
            var workouts = await _workoutManager.GetWorkoutsByWorkoutDayAsync(workoutDayId);
            return Ok(workouts);
        }

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
                return CreatedAtAction(nameof(GetWorkout), new { id = createdWorkout.Id }, createdWorkout);
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

            if (id != workoutDto.Id)
            {
                return BadRequest("ID in URL must match ID in body");
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

        [HttpGet("getExercises/{workoutId}")]
        public IEnumerable<ExerciseExecutionDto> GetExercisesExecutionsFromWorkout(Guid workoutId)
        {
            List<ExerciseExecutionDto> exercisesExecutions = _workoutManager.GetExercisesExecutionsFromWorkout(workoutId);

            return exercisesExecutions;
        }

        [HttpPost("addExerciseExecution/{workoutId}")]
        public IActionResult AddExerciseExecutionToWorkout(Guid workoutId, [FromBody] ExerciseExecutionDto exerciseExecutionDto)
        {
            _workoutManager.AddExerciseExecutionToWorkout(workoutId, exerciseExecutionDto);

            return Ok("The exercise execution has been added to the workout");
        }

        [HttpPost("removeExerciseExecution/{workoutId}/{exerciseId}")]
        public IActionResult RemoveExerciseExecutionFromWorkout(Guid workoutId, Guid exerciseId)
        {
            _workoutManager.RemoveExerciseExecutionFromWorkout(workoutId, exerciseId);

            return Ok("The exercise execution has been removed from the workout");
        }
        */
    }
}
