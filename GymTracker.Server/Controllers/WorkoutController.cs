using GymTracker.Server.Dtos;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutManager WorkoutManager;

        public WorkoutController(IWorkoutManager workoutManager)
        {
            this.WorkoutManager = workoutManager;
        }

        [HttpGet("all")]
        public IEnumerable<Workout> GetWorkouts()
        {
            var workouts = WorkoutManager.GetWorkouts();

            return workouts;
        }

        [HttpGet("{workoutId}")]
        public IActionResult GetWorkout(Guid workoutId)
        {
            var workout = WorkoutManager.GetWorkout(workoutId);

            if (workout == null)
            {
                return NotFound($"Workout with Id '{workoutId}' not found.");
            }

            return Ok(workout);
        }

        [HttpPost("upsert")]
        public IActionResult CreateOrEditWorkout([FromBody] WorkoutDto workout)
        {
            var workoutDef = WorkoutManager.CreateOrEditWorkout(workout);
            
            return Ok(workoutDef);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteWorkout(Guid id)
        {
            WorkoutManager.DeleteWorkout(id);

            return Ok($"The workout has been deleted");
        }

        [HttpGet("getExercisesFromWorkout/{workoutId}")]
        public IEnumerable<Exercise> GetExercisesFromWorkout(Guid workoutId)
        {
            var exercises = WorkoutManager.GetExercisesFromWorkout(workoutId);

            return exercises;
        }

        [HttpPost("addExerciseToWorkout/{workoutId}/{exerciseId}")]
        public IActionResult AddExerciseToWorkout(Guid workoutId, Guid exerciseId)
        {
            WorkoutManager.AddExerciseToWorkout(workoutId, exerciseId);

            return Ok("The exercise has been added to the workout");
        }

        [HttpPost("removeExerciseFromWorkout/{workoutId}/{exerciseId}")]
        public IActionResult RemoveExerciseFromWorkout(Guid workoutId, Guid exerciseId)
        {
            WorkoutManager.RemoveExerciseFromWorkout(workoutId, exerciseId);

            return Ok("The exercise has been removed from the workout");
        }

        [HttpGet("getExercises/{workoutId}")]
        public IEnumerable<ExerciseExecutionDto> GetExercisesExecutionsFromWorkout(Guid workoutId)
        {
            List<ExerciseExecutionDto> exercisesExecutions = WorkoutManager.GetExercisesExecutionsFromWorkout(workoutId);

            return exercisesExecutions;
        }

        [HttpPost("addExerciseExecution/{workoutId}")]
        public IActionResult AddExerciseExecutionToWorkout(Guid workoutId, [FromBody] ExerciseExecutionDto exerciseExecutionDto)
        {
            WorkoutManager.AddExerciseExecutionToWorkout(workoutId, exerciseExecutionDto);

            return Ok("The exercise execution has been added to the workout");
        }

        [HttpPost("removeExerciseExecution/{workoutId}/{exerciseId}")]
        public IActionResult RemoveExerciseExecutionFromWorkout(Guid workoutId, Guid exerciseId)
        {
            WorkoutManager.RemoveExerciseFromWorkout(workoutId, exerciseId);

            return Ok("The exercise execution has been removed from the workout");
        }
    }
}
