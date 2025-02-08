using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineManager RoutineManager;

        public RoutineController(IRoutineManager routineManager)
        {
            this.RoutineManager = routineManager;
        }

        [HttpGet("all")]
        public IEnumerable<Routine> Get()
        {
            var routines = RoutineManager.GetRoutines();

            return routines;
        }

        [HttpGet("{id}")]
        public IActionResult GetRoutine(Guid id)
        {
            var routine = RoutineManager.GetRoutine(id);

            if (routine == null)
            {
                throw new ArgumentException($"No routine found with ID: {routine.Id}");
            }

            return Ok(routine);
        }

        [HttpPost("upsert")]
        public IActionResult CreateOrEditRoutine([FromBody] Routine routine)
        {
            Routine routineDef = this.RoutineManager.UpsertRoutine(routine);

            return Ok(routineDef);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteRoutine(Guid id)
        {
            this.RoutineManager.DeleteRoutine(id);

            return Ok($"The routine has been deleted");
        }
    }
}
