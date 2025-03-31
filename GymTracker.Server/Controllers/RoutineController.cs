using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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
        private readonly ILogger<RoutineController> _logger;

        /// <summary>
        /// Initializes a new instance of RoutineController
        /// </summary>
        /// <param name="routineManager">The routine manager service</param>
        /// <param name="logger">The logger service</param>
        public RoutineController(IRoutineManager routineManager, ILogger<RoutineController> logger)
        {
            _routineManager = routineManager;
            _logger = logger;
        }

        /*
        /// <summary>
        /// Gets all non-deleted routines
        /// </summary>
        /// <returns>A collection of routines</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoutineResponseDto>>> GetRoutines()
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
        public async Task<ActionResult<RoutineResponseDto>> GetRoutine(Guid id)
        {
            var routine = await _routineManager.GetRoutineAsync(id);
            
            if (routine == null)
            {
                return NotFound($"Routine with ID {id} not found");
            }

            return Ok(routine);
        }

        /// <summary>
        /// Creates a new routine
        /// </summary>
        /// <param name="routineDto">The routine data to create</param>
        /// <returns>The created routine</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoutineResponseDto>> CreateRoutine([FromBody] RoutineDto routineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdRoutine = await _routineManager.CreateRoutineAsync(routineDto);
                return CreatedAtAction(nameof(GetRoutine), new { id = createdRoutine.Id }, createdRoutine);
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
        public async Task<ActionResult<RoutineResponseDto>> UpdateRoutine(Guid id, [FromBody] RoutineDto routineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != routineDto.Id)
            {
                return BadRequest("ID in URL must match ID in body");
            }

            try
            {
                var updatedRoutine = await _routineManager.UpdateRoutineAsync(id, routineDto);
                return Ok(updatedRoutine);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Routine with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating routine");
                return StatusCode(500, "An error occurred while updating the routine");
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
            var deleted = await _routineManager.DeleteRoutineAsync(id);
            
            if (!deleted)
            {
                return NotFound($"Routine with ID {id} not found");
            }

            return NoContent();
        }*/
    }
}
