using GymTracker.Server.Controllers;
using GymTracker.Server.Dtos.BodyPart;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymTracker.Server.Tests.Controllers
{
    public class BodyPartControllerTests
    {
        private readonly Mock<IBodyPartManager> _mockBodyPartManager;
        private readonly Mock<ILogger<BodyPartController>> _mockLogger;
        private readonly BodyPartController _controller;

        public BodyPartControllerTests()
        {
            _mockBodyPartManager = new Mock<IBodyPartManager>();
            _mockLogger = new Mock<ILogger<BodyPartController>>();
            _controller = new BodyPartController(_mockBodyPartManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBodyParts_ReturnsOkResult_WithBodyParts()
        {
            // Arrange
            var bodyParts = new List<BodyPartDto>
            {
                new BodyPartDto { id = Guid.NewGuid(), name = "Chest" },
                new BodyPartDto { id = Guid.NewGuid(), name = "Back" }
            };
            _mockBodyPartManager.Setup(m => m.GetBodyPartsAsync()).ReturnsAsync(bodyParts);

            // Act
            var result = await _controller.GetBodyParts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBodyParts = Assert.IsAssignableFrom<IEnumerable<BodyPartDto>>(okResult.Value);
            Assert.Equal(2, returnedBodyParts.Count());
        }
    }
} 