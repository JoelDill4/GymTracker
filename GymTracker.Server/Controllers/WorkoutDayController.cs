using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class WorkoutDayController : ControllerBase
    {
        private readonly IWorkoutDayManager _workoutDayManager;
        private readonly ILogger<WorkoutDayController> _logger;

        public WorkoutDayController(IWorkoutDayManager workoutDayManager, ILogger<WorkoutDayController> logger)
        {
            _workoutDayManager = workoutDayManager;
            _logger = logger;
        }

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutDayResponseDto>>> GetWorkoutDays()
        {
            var workoutDays = await _workoutDayManager.GetWorkoutDaysAsync();
            return Ok(workoutDays);
        }

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

        [HttpGet("routine/{routineId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutDayResponseDto>>> GetWorkoutDaysByRoutine(Guid routineId)
        {
            var workoutDays = await _workoutDayManager.GetWorkoutDaysByRoutineAsync(routineId);
            return Ok(workoutDays);
        }

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
                return CreatedAtAction(nameof(GetWorkoutDay), new { id = createdWorkoutDay.Id }, createdWorkoutDay);
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

            if (id != workoutDayDto.Id)
            {
                return BadRequest("ID in URL must match ID in body");
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

        [HttpGet("getExercisesFromWorkoutDay/{workoutDayId}")]
        public IEnumerable<Exercise> GetExercisesFromWorkoutDay(Guid workoutDayId)
        {
            var exercises = _workoutDayManager.GetExercisesFromWorkoutDay(workoutDayId);

            return exercises;
        }

        [HttpPost("addExerciseToWorkoutDay/{workoutDayId}/{exerciseId}")]
        public IActionResult AddExerciseToWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            _workoutDayManager.AddExerciseToWorkoutDay(workoutDayId, exerciseId);

            return Ok("The exercise has been added to the workoutDay");
        }

        [HttpPost("removeExerciseFromWorkoutDay/{workoutDayId}/{exerciseId}")]
        public IActionResult RemoveExerciseFromWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            _workoutDayManager.RemoveExerciseFromWorkoutDay(workoutDayId, exerciseId);

            return Ok("The exercise has been removed from the workoutDay");
        }*/
    }
}
