using GymTracker.Server.Controllers;
using GymTracker.Server.Dtos.Routine;
using GymTracker.Server.Models;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using GymTracker.Server.Dtos.WorkoutDay;

namespace GymTracker.Server.Tests.Controllers
{
    public class RoutineControllerTests
    {
        private readonly Mock<IRoutineManager> _mockRoutineManager;
        private readonly Mock<IWorkoutDayManager> _mockWorkoutDayManager;
        private readonly Mock<ILogger<RoutineController>> _mockLogger;
        private readonly RoutineController _controller;

        public RoutineControllerTests()
        {
            _mockRoutineManager = new Mock<IRoutineManager>();
            _mockWorkoutDayManager = new Mock<IWorkoutDayManager>();
            _mockLogger = new Mock<ILogger<RoutineController>>();
            _controller = new RoutineController(_mockRoutineManager.Object, _mockWorkoutDayManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetWorkoutDaysByRoutine_ReturnsOkResult_WithWorkoutDays()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var workoutDays = new List<WorkoutDayResponseDto>
            {
                new WorkoutDayResponseDto { id = Guid.NewGuid(), name = "Day 1", description = "First day" },
                new WorkoutDayResponseDto { id = Guid.NewGuid(), name = "Day 2", description = "Second day" }
            };
            _mockWorkoutDayManager.Setup(m => m.GetWorkoutDaysByRoutineAsync(routineId)).ReturnsAsync(workoutDays);

            // Act
            var result = await _controller.GetWorkoutDaysByRoutine(routineId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkoutDays = Assert.IsAssignableFrom<IEnumerable<WorkoutDayResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkoutDays.Count());
        }

        [Fact]
        public async Task GetWorkoutDaysByRoutine_WithNonExistentRoutine_ReturnsEmptyList()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var emptyList = new List<WorkoutDayResponseDto>();
            _mockWorkoutDayManager.Setup(m => m.GetWorkoutDaysByRoutineAsync(routineId)).ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetWorkoutDaysByRoutine(routineId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkoutDays = Assert.IsAssignableFrom<IEnumerable<WorkoutDayResponseDto>>(okResult.Value);
            Assert.Empty(returnedWorkoutDays);
        }

        [Fact]
        public async Task GetRoutines_ReturnsOkResult_WithRoutines()
        {
            // Arrange
            var routines = new List<Routine>
            {
                new Routine { id = Guid.NewGuid(), name = "Routine 1", description = "Description 1" },
                new Routine { id = Guid.NewGuid(), name = "Routine 2", description = "Description 2" }
            };
            _mockRoutineManager.Setup(m => m.GetRoutinesAsync()).ReturnsAsync(routines);

            // Act
            var result = await _controller.GetRoutines();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRoutines = Assert.IsAssignableFrom<IEnumerable<Routine>>(okResult.Value);
            Assert.Equal(2, returnedRoutines.Count());
        }

        [Fact]
        public async Task GetRoutine_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var routine = new Routine { id = routineId, name = "Test Routine", description = "Test Description" };
            _mockRoutineManager.Setup(m => m.GetRoutineAsync(routineId)).ReturnsAsync(routine);

            // Act
            var result = await _controller.GetRoutine(routineId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRoutine = Assert.IsType<Routine>(okResult.Value);
            Assert.Equal(routineId, returnedRoutine.id);
            Assert.Equal(routine.name, returnedRoutine.name);
        }

        [Fact]
        public async Task GetRoutine_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            _mockRoutineManager.Setup(m => m.GetRoutineAsync(routineId)).ReturnsAsync((Routine)null);

            // Act
            var result = await _controller.GetRoutine(routineId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateRoutine_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var routineDto = new RoutineDto 
            { 
                name = "New Routine",
                description = "New Description"
            };
            var createdRoutine = new Routine 
            { 
                id = Guid.NewGuid(),
                name = routineDto.name,
                description = routineDto.description,
                createdAt = DateTime.UtcNow
            };
            _mockRoutineManager.Setup(m => m.CreateRoutineAsync(It.IsAny<Routine>())).ReturnsAsync(createdRoutine);

            // Act
            var result = await _controller.CreateRoutine(routineDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedRoutine = Assert.IsType<Routine>(createdAtResult.Value);
            Assert.Equal(routineDto.name, returnedRoutine.name);
            Assert.Equal(routineDto.description, returnedRoutine.description);
        }

        [Fact]
        public async Task CreateRoutine_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var routineDto = new RoutineDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.CreateRoutine(routineDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateRoutine_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var routineDto = new RoutineDto 
            { 
                name = "Updated Routine",
                description = "Updated Description"
            };
            var updatedRoutine = new Routine 
            { 
                id = routineId,
                name = routineDto.name,
                description = routineDto.description,
                updatedAt = DateTime.UtcNow
            };
            _mockRoutineManager.Setup(m => m.UpdateRoutineAsync(routineId, It.IsAny<Routine>())).ReturnsAsync(updatedRoutine);

            // Act
            var result = await _controller.UpdateRoutine(routineId, routineDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRoutine = Assert.IsType<Routine>(okResult.Value);
            Assert.Equal(routineDto.name, returnedRoutine.name);
            Assert.Equal(routineDto.description, returnedRoutine.description);
        }

        [Fact]
        public async Task UpdateRoutine_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var routineDto = new RoutineDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.UpdateRoutine(routineId, routineDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateRoutine_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            var routineDto = new RoutineDto { name = "Updated Routine", description = "Updated Description" };
            _mockRoutineManager.Setup(m => m.UpdateRoutineAsync(routineId, It.IsAny<Routine>()))
                .ThrowsAsync(new KeyNotFoundException($"Routine with ID {routineId} not found"));

            // Act
            var result = await _controller.UpdateRoutine(routineId, routineDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteRoutine_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            _mockRoutineManager.Setup(m => m.DeleteRoutineAsync(routineId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRoutine(routineId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRoutine_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var routineId = Guid.NewGuid();
            _mockRoutineManager.Setup(m => m.DeleteRoutineAsync(routineId))
                .ThrowsAsync(new KeyNotFoundException($"Routine with ID {routineId} not found"));

            // Act
            var result = await _controller.DeleteRoutine(routineId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
} 