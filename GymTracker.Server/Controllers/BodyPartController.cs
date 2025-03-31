using GymTracker.Server.Dtos.Exercise;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BodyPartController : ControllerBase
    {
        private readonly IBodyPartManager _bodyPartManager;
        private readonly ILogger<BodyPartController> _logger;

        public BodyPartController(IBodyPartManager bodyPartManager, ILogger<BodyPartController> logger)
        {
            _bodyPartManager = bodyPartManager;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BodyPartDto>>> GetBodyParts()
        {
            var bodyParts = await _bodyPartManager.GetBodyPartsAsync();
            return Ok(bodyParts);
        }
    }
} 