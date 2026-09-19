using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Claims.Validation;

namespace Claims.Controllers
{
    /// <summary>HTTP requests for creating, retrieving and deleting insurance covers </summary>
    [ApiController]
    [Route("[controller]")]
    public class CoversController : ControllerBase
    {
        private readonly ILogger<CoversController> _logger;
        private readonly ICoversService _coversService;

        public CoversController(ICoversService coversService, ILogger<CoversController> logger)
        {
            _coversService = coversService;
            _logger = logger;
        }

        [HttpPost("compute")]
        public ActionResult ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            return Ok(_coversService.ComputePremium(startDate, endDate, coverType));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
        {
            var results = await _coversService.GetCoversAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cover>> GetAsync(string id)
        {
            var result = await _coversService.GetCoverAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Cover cover)
        {
            try
            {
                var created = await _coversService.CreateCoverAsync(cover);
                return Ok(created);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _coversService.DeleteCoverAsync(id);
        }
    }
}