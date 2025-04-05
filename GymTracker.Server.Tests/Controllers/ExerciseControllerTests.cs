using GymTracker.Server.Controllers;
using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymTracker.Server.Tests.Controllers
{
    public class ExerciseControllerTests
    {
        private readonly Mock<IExerciseManager> _mockExerciseManager;
        private readonly Mock<ILogger<ExerciseController>> _mockLogger;
        private readonly ExerciseController _controller;

        public ExerciseControllerTests()
        {
            _mockExerciseManager = new Mock<IExerciseManager>();
            _mockLogger = new Mock<ILogger<ExerciseController>>();
            _controller = new ExerciseController(_mockExerciseManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetExercises_ReturnsOkResult_WithExercises()
        {
            // Arrange
            var exercises = new List<ExerciseResponseDto>
            {
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Exercise 1" },
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Exercise 2" }
            };
            _mockExerciseManager.Setup(m => m.GetExercisesAsync()).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExercises();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercises = Assert.IsAssignableFrom<IEnumerable<ExerciseResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedExercises.Count());
        }

        [Fact]
        public async Task GetExercise_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var exercise = new ExerciseResponseDto { id = exerciseId, name = "Test Exercise" };
            _mockExerciseManager.Setup(m => m.GetExerciseAsync(exerciseId)).ReturnsAsync(exercise);

            // Act
            var result = await _controller.GetExercise(exerciseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercise = Assert.IsType<ExerciseResponseDto>(okResult.Value);
            Assert.Equal(exerciseId, returnedExercise.id);
        }

        [Fact]
        public async Task GetExercise_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            _mockExerciseManager.Setup(m => m.GetExerciseAsync(exerciseId)).ReturnsAsync((ExerciseResponseDto)null);

            // Act
            var result = await _controller.GetExercise(exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetExerciseByName_ReturnsOkResult_WithMatchingExercises()
        {
            // Arrange
            var searchName = "push";
            var exercises = new List<ExerciseResponseDto>
            {
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Push-ups" },
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Push Press" }
            };
            _mockExerciseManager.Setup(m => m.GetExercisesByNameAsync(searchName)).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExerciseByName(searchName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercises = Assert.IsAssignableFrom<IEnumerable<ExerciseResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedExercises.Count());
        }

        [Fact]
        public async Task GetExercisesByBodyPart_ReturnsOkResult_WithExercises()
        {
            // Arrange
            var bodyPartId = Guid.NewGuid();
            var exercises = new List<ExerciseResponseDto>
            {
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Exercise 1" },
                new ExerciseResponseDto { id = Guid.NewGuid(), name = "Exercise 2" }
            };
            _mockExerciseManager.Setup(m => m.GetExercisesByBodyPartAsync(bodyPartId)).ReturnsAsync(exercises);

            // Act
            var result = await _controller.GetExercisesByBodyPart(bodyPartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercises = Assert.IsAssignableFrom<IEnumerable<ExerciseResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedExercises.Count());
        }

        [Fact]
        public async Task CreateExercise_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var exerciseDto = new ExerciseDto { name = "New Exercise", description = "Description", fk_bodypart = Guid.NewGuid() };
            var createdExercise = new ExerciseResponseDto { id = Guid.NewGuid(), name = exerciseDto.name };
            _mockExerciseManager.Setup(m => m.CreateExerciseAsync(exerciseDto)).ReturnsAsync(createdExercise);

            // Act
            var result = await _controller.CreateExercise(exerciseDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedExercise = Assert.IsType<ExerciseResponseDto>(createdAtResult.Value);
            Assert.Equal(exerciseDto.name, returnedExercise.name);
        }

        [Fact]
        public async Task CreateExercise_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var exerciseDto = new ExerciseDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.CreateExercise(exerciseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateExercise_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var exerciseDto = new ExerciseDto { name = "Updated Exercise", description = "Updated Description", fk_bodypart = Guid.NewGuid() };
            var updatedExercise = new ExerciseResponseDto { id = exerciseId, name = exerciseDto.name };
            _mockExerciseManager.Setup(m => m.UpdateExerciseAsync(exerciseId, exerciseDto)).ReturnsAsync(updatedExercise);

            // Act
            var result = await _controller.UpdateExercise(exerciseId, exerciseDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedExercise = Assert.IsType<ExerciseResponseDto>(okResult.Value);
            Assert.Equal(exerciseDto.name, returnedExercise.name);
        }

        [Fact]
        public async Task UpdateExercise_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var exerciseDto = new ExerciseDto(); // Invalid due to missing required fields
            _controller.ModelState.AddModelError("name", "Name is required");

            // Act
            var result = await _controller.UpdateExercise(exerciseId, exerciseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateExercise_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var exerciseDto = new ExerciseDto { name = "Updated Exercise", description = "Updated Description", fk_bodypart = Guid.NewGuid() };
            _mockExerciseManager.Setup(m => m.UpdateExerciseAsync(exerciseId, exerciseDto))
                .ThrowsAsync(new KeyNotFoundException($"Exercise with ID {exerciseId} not found"));

            // Act
            var result = await _controller.UpdateExercise(exerciseId, exerciseDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteExercise_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            _mockExerciseManager.Setup(m => m.DeleteExerciseAsync(exerciseId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteExercise(exerciseId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteExercise_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            _mockExerciseManager.Setup(m => m.DeleteExerciseAsync(exerciseId))
                .ThrowsAsync(new KeyNotFoundException($"Exercise with ID {exerciseId} not found"));

            // Act
            var result = await _controller.DeleteExercise(exerciseId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
} 