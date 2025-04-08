using GymTracker.Server.Dtos.BodyPart;
using GymTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GymTracker.Server.Controllers
{
    /// <summary>
    /// Controller for managing body parts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BodyPartController : ControllerBase
    {
        private readonly IBodyPartManager _bodyPartManager;
        private readonly ILogger<BodyPartController> _logger;

        /// <summary>
        /// Initializes a new instance of BodyPartController
        /// </summary>
        /// <param name="bodyPartManager">The body part manager service</param>
        /// <param name="logger">The logger service</param>
        public BodyPartController(IBodyPartManager bodyPartManager, ILogger<BodyPartController> logger)
        {
            _bodyPartManager = bodyPartManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets all body parts
        /// </summary>
        /// <returns>A collection of body parts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BodyPartDto>>> GetBodyParts()
        {
            var bodyParts = await _bodyPartManager.GetBodyPartsAsync();
            return Ok(bodyParts);
        }
    }
} 