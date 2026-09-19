using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Claims.Validation;
namespace Claims.Controllers
{
    /// <summary> HTTP requests for creating, retrieving and deleting insurance claims</summary>
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimsService _claimsService;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService)
        {
            _logger = logger;
            _claimsService = claimsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Claim>> GetAsync()
        {
            return await _claimsService.GetClaimsAsync();
        }

        [HttpGet("{id}")]
        public async Task<Claim> GetAsync(string id)
        {
            return await _claimsService.GetClaimAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim claim)
        {
            try
            {
                var created = await _claimsService.CreateClaimAsync(claim);
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
            await _claimsService.DeleteClaimAsync(id);
        }
    }
}