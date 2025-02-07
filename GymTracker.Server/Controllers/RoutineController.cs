using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineManager routineManager;

        public RoutineController(IRoutineManager routineManager)
        {
            this.routineManager = routineManager;
        }

        [HttpGet("all")]
        public IEnumerable<Routine> Get()
        {
            var routines = routineManager.GetRoutines();

            return routines;
        }

        [HttpGet("{name}")]
        public IActionResult GetRoutineByName(string name)
        {
            var routine = routineManager.GetRoutine(name);

            if (routine == null)
            {
                return NotFound($"Routine with name '{name}' not found.");
            }

            return Ok(routine);
        }

        [HttpPut("edit/{name}")]
        public IActionResult EditRoutine(string name, string newName)
        {
            Routine routine = this.routineManager.EditRoutine(name, newName);

            return Ok(routine);
        }

        [HttpPost("add/{name}")]
        public IActionResult CreateRoutine(string name)
        {
            Routine routine = this.routineManager.CreateRoutine(name);

            return Ok(routine);
        }

        [HttpDelete("delete/{name}")]
        public IActionResult DeleteRoutine(string name)
        {
            this.routineManager.DeleteRoutine(name);

            return Ok($"The routine \"{name}\" has been deleted");
        }
    }
}
