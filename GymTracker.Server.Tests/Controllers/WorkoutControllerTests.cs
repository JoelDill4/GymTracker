using GymTracker.Server.Controllers;
using GymTracker.Server.Dtos.ExerciseSet;
using GymTracker.Server.Dtos.Workout;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymTracker.Server.Tests.Controllers
{
    public class WorkoutControllerTests
    {
        private readonly Mock<IWorkoutManager> _mockWorkoutManager;
        private readonly Mock<ILogger<WorkoutController>> _mockLogger;
        private readonly WorkoutController _controller;

        public WorkoutControllerTests()
        {
            _mockWorkoutManager = new Mock<IWorkoutManager>();
            _mockLogger = new Mock<ILogger<WorkoutController>>();
            _controller = new WorkoutController(_mockWorkoutManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetWorkouts_ReturnsOkResult_WithWorkouts()
        {
            // Arrange
            var workouts = new List<WorkoutResponseDto>
            {
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow },
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow }
            };
            _mockWorkoutManager.Setup(m => m.GetWorkoutsAsync()).ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetWorkouts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkouts = Assert.IsAssignableFrom<IEnumerable<WorkoutResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkouts.Count());
        }

        [Fact]
        public async Task GetWorkout_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var workout = new WorkoutResponseDto { id = workoutId, workoutDate = DateTime.UtcNow };
            _mockWorkoutManager.Setup(m => m.GetWorkoutAsync(workoutId)).ReturnsAsync(workout);

            // Act
            var result = await _controller.GetWorkout(workoutId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkout = Assert.IsType<WorkoutResponseDto>(okResult.Value);
            Assert.Equal(workoutId, returnedWorkout.id);
        }

        [Fact]
        public async Task GetWorkout_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            _mockWorkoutManager.Setup(m => m.GetWorkoutAsync(workoutId)).ReturnsAsync((WorkoutResponseDto)null);

            // Act
            var result = await _controller.GetWorkout(workoutId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetWorkoutsByDateRange_ReturnsOkResult_WithWorkouts()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow;
            var workouts = new List<WorkoutResponseDto>
            {
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow.AddDays(-3) },
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow.AddDays(-1) }
            };
            _mockWorkoutManager.Setup(m => m.GetWorkoutsByDateRangeAsync(startDate, endDate)).ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetWorkoutsByDateRange(startDate, endDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkouts = Assert.IsAssignableFrom<IEnumerable<WorkoutResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkouts.Count());
        }

        [Fact]
        public async Task GetWorkoutsByWorkoutDay_ReturnsOkResult_WithWorkouts()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var workouts = new List<WorkoutResponseDto>
            {
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow },
                new WorkoutResponseDto { id = Guid.NewGuid(), workoutDate = DateTime.UtcNow }
            };
            _mockWorkoutManager.Setup(m => m.GetWorkoutsByWorkoutDayAsync(workoutDayId)).ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetWorkoutsByWorkoutDay(workoutDayId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkouts = Assert.IsAssignableFrom<IEnumerable<WorkoutResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkouts.Count());
        }

        [Fact]
        public async Task CreateWorkout_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var workoutDto = new WorkoutDto 
            { 
                workoutDate = DateTime.UtcNow,
                observations = "Test workout",
                workoutDayId = Guid.NewGuid()
            };
            var createdWorkout = new WorkoutResponseDto 
            { 
                id = Guid.NewGuid(),
                workoutDate = workoutDto.workoutDate,
                observations = workoutDto.observations
            };
            _mockWorkoutManager.Setup(m => m.CreateWorkoutAsync(workoutDto)).ReturnsAsync(createdWorkout);

            // Act
            var result = await _controller.CreateWorkout(workoutDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedWorkout = Assert.IsType<WorkoutResponseDto>(createdAtResult.Value);
            Assert.Equal(workoutDto.workoutDate, returnedWorkout.workoutDate);
            Assert.Equal(workoutDto.observations, returnedWorkout.observations);
        }

        [Fact]
        public async Task CreateWorkout_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var workoutDto = new WorkoutDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("workoutDate", "Workout date is required");

            // Act
            var result = await _controller.CreateWorkout(workoutDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateWorkout_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var workoutDto = new WorkoutDto 
            { 
                workoutDate = DateTime.UtcNow,
                observations = "Updated workout",
                workoutDayId = Guid.NewGuid()
            };
            var updatedWorkout = new WorkoutResponseDto 
            { 
                id = workoutId,
                workoutDate = workoutDto.workoutDate,
                observations = workoutDto.observations
            };
            _mockWorkoutManager.Setup(m => m.UpdateWorkoutAsync(workoutId, workoutDto)).ReturnsAsync(updatedWorkout);

            // Act
            var result = await _controller.UpdateWorkout(workoutId, workoutDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkout = Assert.IsType<WorkoutResponseDto>(okResult.Value);
            Assert.Equal(workoutDto.workoutDate, returnedWorkout.workoutDate);
            Assert.Equal(workoutDto.observations, returnedWorkout.observations);
        }

        [Fact]
        public async Task UpdateWorkout_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var workoutDto = new WorkoutDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("workoutDate", "Workout date is required");

            // Act
            var result = await _controller.UpdateWorkout(workoutId, workoutDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateWorkout_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var workoutDto = new WorkoutDto 
            { 
                workoutDate = DateTime.UtcNow,
                observations = "Updated workout",
                workoutDayId = Guid.NewGuid()
            };
            _mockWorkoutManager.Setup(m => m.UpdateWorkoutAsync(workoutId, workoutDto))
                .ThrowsAsync(new KeyNotFoundException($"Workout with ID {workoutId} not found"));

            // Act
            var result = await _controller.UpdateWorkout(workoutId, workoutDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteWorkout_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            _mockWorkoutManager.Setup(m => m.DeleteWorkoutAsync(workoutId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteWorkout(workoutId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteWorkout_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            _mockWorkoutManager.Setup(m => m.DeleteWorkoutAsync(workoutId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteWorkout(workoutId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetExerciseSetsFromWorkout_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSets = new List<ExerciseSetDto>
            {
                new ExerciseSetDto { order = 1, weight = 100, reps = 10, exerciseId = Guid.NewGuid() },
                new ExerciseSetDto { order = 2, weight = 120, reps = 8, exerciseId = Guid.NewGuid() }
            };
            _mockWorkoutManager.Setup(m => m.GetExerciseSetsFromWorkout(workoutId)).Returns(exerciseSets);

            // Act
            var result = _controller.GetExerciseSetsFromWorkout(workoutId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExerciseSets = Assert.IsAssignableFrom<IEnumerable<ExerciseSetDto>>(okResult.Value);
            Assert.Equal(2, returnedExerciseSets.Count());
        }

        [Fact]
        public void GetExerciseSetsFromWorkout_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            _mockWorkoutManager.Setup(m => m.GetExerciseSetsFromWorkout(workoutId))
                .Throws(new KeyNotFoundException($"Workout with ID {workoutId} not found"));

            // Act
            var result = _controller.GetExerciseSetsFromWorkout(workoutId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void AddExerciseSetToWorkout_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSetDto = new ExerciseSetDto 
            { 
                order = 1,
                weight = 100,
                reps = 10,
                exerciseId = Guid.NewGuid()
            };

            // Act
            var result = _controller.AddExerciseSetToWorkout(workoutId, exerciseSetDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddExerciseSetToWorkout_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSetDto = new ExerciseSetDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("order", "Order is required");

            // Act
            var result = _controller.AddExerciseSetToWorkout(workoutId, exerciseSetDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddExerciseSetToWorkout_WithInvalidWorkoutId_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSetDto = new ExerciseSetDto 
            { 
                order = 1,
                weight = 100,
                reps = 10,
                exerciseId = Guid.NewGuid()
            };
            _mockWorkoutManager.Setup(m => m.AddExerciseSetToWorkout(workoutId, exerciseSetDto))
                .Throws(new KeyNotFoundException($"Workout with ID {workoutId} not found"));

            // Act
            var result = _controller.AddExerciseSetToWorkout(workoutId, exerciseSetDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveExerciseSetFromWorkout_WithValidIds_ReturnsOkResult()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSetId = Guid.NewGuid();

            // Act
            var result = _controller.RemoveExerciseSetFromWorkout(workoutId, exerciseSetId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void RemoveExerciseSetFromWorkout_WithInvalidIds_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseSetId = Guid.NewGuid();
            _mockWorkoutManager.Setup(m => m.RemoveExerciseSetFromWorkout(workoutId, exerciseSetId))
                .Throws(new KeyNotFoundException($"Exercise set with ID {exerciseSetId} not found in workout {workoutId}"));

            // Act
            var result = _controller.RemoveExerciseSetFromWorkout(workoutId, exerciseSetId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AssignExerciseSetsOfExerciseToWorkout_ValidRequest_ReturnsOk()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var exerciseSets = new List<ExerciseSetDto>
            {
                new ExerciseSetDto { order = 1, reps = 10, weight = 100, exerciseId = exerciseId },
                new ExerciseSetDto { order = 2, reps = 8, weight = 110, exerciseId = exerciseId }
            };

            // Act
            var result = _controller.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("The exercise sets have been assigned to the exercise in the workout", okResult.Value);
            _mockWorkoutManager.Verify(x => x.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets), Times.Once);
        }

        [Fact]
        public void AssignExerciseSetsOfExerciseToWorkout_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var exerciseSets = new List<ExerciseSetDto>();
            _controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = _controller.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _mockWorkoutManager.Verify(x => x.AssignExerciseSetsOfExerciseToWorkout(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<ExerciseSetDto>>()), Times.Never);
        }

        [Fact]
        public void AssignExerciseSetsOfExerciseToWorkout_WorkoutNotFound_ReturnsNotFound()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var exerciseSets = new List<ExerciseSetDto>
            {
                new ExerciseSetDto { order = 1, reps = 10, weight = 100, exerciseId = exerciseId }
            };

            _mockWorkoutManager
                .Setup(x => x.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets))
                .Throws(new KeyNotFoundException("Workout not found"));

            // Act
            var result = _controller.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout not found", notFoundResult.Value);
            _mockWorkoutManager.Verify(x => x.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets), Times.Once);
        }

        [Fact]
        public void AssignExerciseSetsOfExerciseToWorkout_ExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var exerciseSets = new List<ExerciseSetDto>
            {
                new ExerciseSetDto { order = 1, reps = 10, weight = 100, exerciseId = exerciseId }
            };

            _mockWorkoutManager
                .Setup(x => x.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = _controller.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while assigning the exercise sets to the exercise in the workout", statusCodeResult.Value);
            _mockWorkoutManager.Verify(x => x.AssignExerciseSetsOfExerciseToWorkout(workoutId, exerciseId, exerciseSets), Times.Once);
        }
    }
} 