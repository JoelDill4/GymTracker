using GymTracker.Server.Dtos;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutDayController : ControllerBase
    {
        private readonly IWorkoutDayManager WorkoutdayManager;

        public WorkoutDayController(IWorkoutDayManager workoutdayManager)
        {
            this.WorkoutdayManager = workoutdayManager;
        }

        [HttpGet("all")]
        public IEnumerable<WorkoutDay> GetWorkoutDays()
        {
            var workoutDays = WorkoutdayManager.GetWorkoutDays();

            return workoutDays;
        }

        [HttpGet("{workoutdayId}")]
        public IActionResult GetWorkoutDay(Guid id)
        {
            var workoutday = WorkoutdayManager.GetWorkoutDay(id);

            if (workoutday == null)
            {
                return NotFound($"WorkoutDay with Id '{id}' not found.");
            }

            return Ok(workoutday);
        }

        [HttpPost("upsert")]
        public IActionResult CreateOrEditWorkoutDay([FromBody] WorkoutDayDto workoutDay)
        {
            WorkoutDay workoutday = WorkoutdayManager.CreateOrEditWorkoutDay(workoutDay);
            
            return Ok(workoutday);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteWorkoutDay(Guid id)
        {
            WorkoutdayManager.DeleteWorkoutDay(id);

            return Ok($"The workoutday has been deleted");
        }

        [HttpGet("getExercisesFromWorkoutDay/{workoutDayId}")]
        public IEnumerable<Exercise> GetExercisesFromWorkoutDay(Guid workoutDayId)
        {
            var exercises = WorkoutdayManager.GetExercisesFromWorkoutDay(workoutDayId);

            return exercises;
        }

        [HttpPost("addExerciseToWorkoutDay/{workoutDayId}/{exerciseId}")]
        public IActionResult AddExerciseToWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            WorkoutdayManager.AddExerciseToWorkoutDay(workoutDayId, exerciseId);

            return Ok("The exercise has been added to the workoutDay");
        }

        [HttpPost("removeExerciseFromWorkoutDay/{workoutDayId}/{exerciseId}")]
        public IActionResult RemoveExerciseFromWorkoutDay(Guid workoutDayId, Guid exerciseId)
        {
            WorkoutdayManager.RemoveExerciseFromWorkoutDay(workoutDayId, exerciseId);

            return Ok("The exercise has been removed from the workoutDay");
        }
    }
}
