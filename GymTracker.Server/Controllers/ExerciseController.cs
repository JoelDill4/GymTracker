using GymTracker.Server.Dtos;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseManager ExerciseManager;

        public ExerciseController(IExerciseManager exerciseManager)
        {
            this.ExerciseManager = exerciseManager;
        }

        [HttpGet("all")]
        public IEnumerable<Exercise> GetExercises()
        {
            var exercises = ExerciseManager.GetExercises();

            return exercises;
        }

        [HttpGet("{id}")]
        public IActionResult GetExercise(Guid id)
        {
            var exercise = ExerciseManager.GetExercise(id);

            if (exercise == null)
            {
                return NotFound($"Exercise with Id '{id}' not found.");
            }

            return Ok(exercise);
        }

        [HttpPost("upsert")]
        public IActionResult CreateOrEditExercise([FromBody] ExerciseDto exerciseDto)
        {
            Exercise exercise = ExerciseManager.CreateOrEditExercise(exerciseDto);

            return Ok(exercise);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteExercise(Guid id)
        {
            ExerciseManager.DeleteExercise(id);

            return Ok($"The exercise has been deleted");
        }
    }
}
