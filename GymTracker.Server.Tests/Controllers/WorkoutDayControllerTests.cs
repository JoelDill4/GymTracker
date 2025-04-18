using GymTracker.Server.Controllers;
using GymTracker.Server.Dtos.BodyPart;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Dtos.WorkoutDay;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GymTracker.Server.Tests.Controllers
{
    public class WorkoutDayControllerTests
    {
        private readonly Mock<IWorkoutDayManager> _mockWorkoutDayManager;
        private readonly Mock<ILogger<WorkoutDayController>> _mockLogger;
        private readonly WorkoutDayController _controller;

        public WorkoutDayControllerTests()
        {
            _mockWorkoutDayManager = new Mock<IWorkoutDayManager>();
            _mockLogger = new Mock<ILogger<WorkoutDayController>>();
            _controller = new WorkoutDayController(_mockWorkoutDayManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetWorkoutDays_ReturnsOkResult_WithWorkoutDays()
        {
            // Arrange
            var workoutDays = new List<WorkoutDayResponseDto>
            {
                new WorkoutDayResponseDto { id = Guid.NewGuid(), name = "Day 1" },
                new WorkoutDayResponseDto { id = Guid.NewGuid(), name = "Day 2" }
            };
            _mockWorkoutDayManager.Setup(m => m.GetWorkoutDaysAsync()).ReturnsAsync(workoutDays);

            // Act
            var result = await _controller.GetWorkoutDays();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkoutDays = Assert.IsAssignableFrom<IEnumerable<WorkoutDayResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkoutDays.Count());
        }

        [Fact]
        public async Task GetWorkoutDay_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var workoutDay = new WorkoutDayResponseDto { id = workoutDayId, name = "Test Day" };
            _mockWorkoutDayManager.Setup(m => m.GetWorkoutDayAsync(workoutDayId)).ReturnsAsync(workoutDay);

            // Act
            var result = await _controller.GetWorkoutDay(workoutDayId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkoutDay = Assert.IsType<WorkoutDayResponseDto>(okResult.Value);
            Assert.Equal(workoutDayId, returnedWorkoutDay.id);
        }

        [Fact]
        public async Task GetWorkoutDay_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.GetWorkoutDayAsync(workoutDayId)).ReturnsAsync((WorkoutDayResponseDto)null);

            // Act
            var result = await _controller.GetWorkoutDay(workoutDayId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateWorkoutDay_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var workoutDayDto = new WorkoutDayDto 
            { 
                name = "New Day",
                description = "New Description",
                routineId = Guid.NewGuid()
            };
            var createdWorkoutDay = new WorkoutDayResponseDto 
            { 
                id = Guid.NewGuid(),
                name = workoutDayDto.name,
                description = workoutDayDto.description,
                createdAt = DateTime.UtcNow
            };
            _mockWorkoutDayManager.Setup(m => m.CreateWorkoutDayAsync(workoutDayDto)).ReturnsAsync(createdWorkoutDay);

            // Act
            var result = await _controller.CreateWorkoutDay(workoutDayDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedWorkoutDay = Assert.IsType<WorkoutDayResponseDto>(createdAtResult.Value);
            Assert.Equal(workoutDayDto.name, returnedWorkoutDay.name);
            Assert.Equal(workoutDayDto.description, returnedWorkoutDay.description);
        }

        [Fact]
        public async Task CreateWorkoutDay_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var workoutDayDto = new WorkoutDayDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.CreateWorkoutDay(workoutDayDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateWorkoutDay_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var workoutDayDto = new WorkoutDayDto 
            { 
                name = "Updated Day",
                description = "Updated Description",
                routineId = Guid.NewGuid()
            };
            var updatedWorkoutDay = new WorkoutDayResponseDto 
            { 
                id = workoutDayId,
                name = workoutDayDto.name,
                description = workoutDayDto.description,
                updatedAt = DateTime.UtcNow
            };
            _mockWorkoutDayManager.Setup(m => m.UpdateWorkoutDayAsync(workoutDayId, workoutDayDto)).ReturnsAsync(updatedWorkoutDay);

            // Act
            var result = await _controller.UpdateWorkoutDay(workoutDayId, workoutDayDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkoutDay = Assert.IsType<WorkoutDayResponseDto>(okResult.Value);
            Assert.Equal(workoutDayDto.name, returnedWorkoutDay.name);
            Assert.Equal(workoutDayDto.description, returnedWorkoutDay.description);
        }

        [Fact]
        public async Task UpdateWorkoutDay_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var workoutDayDto = new WorkoutDayDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.UpdateWorkoutDay(workoutDayId, workoutDayDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateWorkoutDay_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var workoutDayDto = new WorkoutDayDto 
            { 
                name = "Updated Day",
                description = "Updated Description",
                routineId = Guid.NewGuid()
            };
            _mockWorkoutDayManager.Setup(m => m.UpdateWorkoutDayAsync(workoutDayId, workoutDayDto))
                .ThrowsAsync(new KeyNotFoundException($"Workout day with ID {workoutDayId} not found"));

            // Act
            var result = await _controller.UpdateWorkoutDay(workoutDayId, workoutDayDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetExercisesFromWorkoutDay_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exercises = new List<ExerciseResponseDto>
            {
                new ExerciseResponseDto
                {
                    id = Guid.NewGuid(),
                    name = "Bench Press",
                    description = "Chest exercise",
                    bodyPart = new BodyPartDto
                    {
                        id = Guid.NewGuid(),
                        name = "Chest"
                    }
                },
                new ExerciseResponseDto
                {
                    id = Guid.NewGuid(),
                    name = "Squats",
                    description = "Leg exercise",
                    bodyPart = new BodyPartDto
                    {
                        id = Guid.NewGuid(),
                        name = "Legs"
                    }
                }
            };
            _mockWorkoutDayManager.Setup(m => m.GetExercisesFromWorkoutDayAsync(workoutDayId))
                .ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExercisesFromWorkoutDay(workoutDayId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercises = Assert.IsAssignableFrom<IEnumerable<ExerciseResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedExercises.Count());
        }

        [Fact]
        public async Task GetExercisesFromWorkoutDay_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.GetExercisesFromWorkoutDayAsync(workoutDayId))
                .ThrowsAsync(new KeyNotFoundException($"Workout day with ID {workoutDayId} not found"));

            // Act
            var result = await _controller.GetExercisesFromWorkoutDay(workoutDayId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task AssignExercisesOfWorkoutDay_WithValidIds_ReturnsNoContent()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exercisesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockWorkoutDayManager.Setup(m => m.AssignExercisesToWorkoutDayAsync(workoutDayId, exercisesIds))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AssignExercisesOfWorkoutDay(workoutDayId, exercisesIds);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AssignExercisesOfWorkoutDay_WithInvalidWorkoutDayId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exercisesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockWorkoutDayManager.Setup(m => m.AssignExercisesToWorkoutDayAsync(workoutDayId, exercisesIds))
                .ThrowsAsync(new KeyNotFoundException($"Workout day with ID {workoutDayId} not found"));

            // Act
            var result = await _controller.AssignExercisesOfWorkoutDay(workoutDayId, exercisesIds);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AssignExercisesOfWorkoutDay_WithInvalidExercisesId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exercisesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockWorkoutDayManager.Setup(m => m.AssignExercisesToWorkoutDayAsync(workoutDayId, exercisesIds))
                .ThrowsAsync(new KeyNotFoundException($"Exercises with IDs {string.Join(", ", exercisesIds)} not found"));

            // Act
            var result = await _controller.AssignExercisesOfWorkoutDay(workoutDayId, exercisesIds);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddExerciseToWorkoutDay_WithValidIds_ReturnsNoContent()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.AddExerciseToWorkoutDayAsync(workoutDayId, exerciseId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddExerciseToWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddExerciseToWorkoutDay_WithInvalidWorkoutDayId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.AddExerciseToWorkoutDayAsync(workoutDayId, exerciseId))
                .ThrowsAsync(new KeyNotFoundException($"Workout day with ID {workoutDayId} not found"));

            // Act
            var result = await _controller.AddExerciseToWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddExerciseToWorkoutDay_WithInvalidExerciseId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.AddExerciseToWorkoutDayAsync(workoutDayId, exerciseId))
                .ThrowsAsync(new KeyNotFoundException($"Exercise with ID {exerciseId} not found"));

            // Act
            var result = await _controller.AddExerciseToWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RemoveExerciseFromWorkoutDay_WithValidIds_ReturnsNoContent()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.RemoveExerciseFromWorkoutDayAsync(workoutDayId, exerciseId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveExerciseFromWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveExerciseFromWorkoutDay_WithInvalidWorkoutDayId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.RemoveExerciseFromWorkoutDayAsync(workoutDayId, exerciseId))
                .ThrowsAsync(new KeyNotFoundException($"Workout day with ID {workoutDayId} not found"));

            // Act
            var result = await _controller.RemoveExerciseFromWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RemoveExerciseFromWorkoutDay_WithInvalidExerciseId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.RemoveExerciseFromWorkoutDayAsync(workoutDayId, exerciseId))
                .ThrowsAsync(new KeyNotFoundException($"Exercise with ID {exerciseId} not found in workout day {workoutDayId}"));

            // Act
            var result = await _controller.RemoveExerciseFromWorkoutDay(workoutDayId, exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteWorkoutDay_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.DeleteWorkoutDayAsync(workoutDayId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteWorkoutDay(workoutDayId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteWorkoutDay_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var workoutDayId = Guid.NewGuid();
            _mockWorkoutDayManager.Setup(m => m.DeleteWorkoutDayAsync(workoutDayId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteWorkoutDay(workoutDayId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
} 