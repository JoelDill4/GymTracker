using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using GymTracker.Server.Models;
using GymTracker.Server.Dtos.WorkoutDay;

namespace GymTracker.Server.Controllers
{
    /// <summary>
    /// Controller for managing workout routines
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineManager _routineManager;
        private readonly IWorkoutDayManager _workoutDayManager;
        private readonly ILogger<RoutineController> _logger;

        /// <summary>
        /// Initializes a new instance of RoutineController
        /// </summary>
        /// <param name="routineManager">The routine manager service</param>
        /// <param name="logger">The logger service</param>
        public RoutineController(IRoutineManager routineManager, IWorkoutDayManager workoutDayManager, ILogger<RoutineController> logger)
        {
            _routineManager = routineManager;
            _workoutDayManager = workoutDayManager;
            _logger = logger;
        }

        
        /// <summary>
        /// Gets all non-deleted routines
        /// </summary>
        /// <returns>A collection of routines</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Routine>>> GetRoutines()
        {
            var routines = await _routineManager.GetRoutinesAsync();
            return Ok(routines);
        }

        /// <summary>
        /// Gets a specific routine by ID
        /// </summary>
        /// <param name="id">The ID of the routine to retrieve</param>
        /// <returns>The routine if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Routine>> GetRoutine(Guid id)
        {
            var routine = await _routineManager.GetRoutineAsync(id);
            
            if (routine == null)
            {
                return NotFound($"Routine with ID {id} not found");
            }

            return Ok(routine);
        }

        [HttpGet("workoutDays/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutDayResponseDto>>> GetWorkoutDaysByRoutine(Guid id)
        {
            var workoutDays = await _workoutDayManager.GetWorkoutDaysByRoutineAsync(id);
            return Ok(workoutDays);
        }

        /// <summary>
        /// Creates a new routine
        /// </summary>
        /// <param name="routineDto">The routine data to create</param>
        /// <returns>The created routine</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Routine>> CreateRoutine(RoutineDto routineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var routine = new Routine
                {
                    name = routineDto.name,
                    description = routineDto.description
                };

                var createdRoutine = await _routineManager.CreateRoutineAsync(routine);
                return CreatedAtAction(nameof(GetRoutine), new { id = createdRoutine.id }, createdRoutine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating routine");
                return StatusCode(500, "An error occurred while creating the routine");
            }
        }

        /// <summary>
        /// Updates an existing routine
        /// </summary>
        /// <param name="id">The ID of the routine to update</param>
        /// <param name="routineDto">The updated routine data</param>
        /// <returns>The updated routine</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Routine>> UpdateRoutine(Guid id, RoutineDto routineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var routine = new Routine
                {
                    name = routineDto.name,
                    description = routineDto.description
                };

                var updatedRoutine = await _routineManager.UpdateRoutineAsync(id, routine);
                return Ok(updatedRoutine);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Routine with ID {id} not found");
            }
        }

        /// <summary>
        /// Deletes a routine
        /// </summary>
        /// <param name="id">The ID of the routine to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoutine(Guid id)
        {
            try
            {
                await _routineManager.DeleteRoutineAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Routine with ID {id} not found");
            }
        }
    }
}
