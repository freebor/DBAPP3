using DBAPP3.Models.DTOs;
using DBAPP3.Services;
using Microsoft.AspNetCore.Mvc;

namespace DBAPP3.Controllers
{
    [ApiController]
    // api/CountryController/Get
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {

        private readonly ILogger<CountryController> _logger;
        private readonly IThirdPartyService _thirdPartyService;

        public CountryController(ILogger<CountryController> logger, IThirdPartyService thirdPartyService)
        {
            _logger = logger;
            _thirdPartyService = thirdPartyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _thirdPartyService.GetCountryName();
            return Ok(result);
        }

        [HttpPost("GetByName")]
        public async Task<IActionResult> GetCountryByName(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return BadRequest("Country name not provided");
            }
            var result = await _thirdPartyService.GetCountryByName(request);
            return Ok(result);
        }

        [HttpGet("GetCurrency")]
        public async Task<ActionResult> GetCurrency(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return BadRequest("please input a valid currency-code");
            }
            var result = await _thirdPartyService.GetCurrency(request);
            return Ok(result);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchCountry(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var result = await _thirdPartyService.SearchCountry(searchTerm);
            if (result == null || !result.Any())
            {
                return NotFound($"No countries found matching '{searchTerm}'.");
            }
            return Ok(result);
        }
        [HttpPost("refresh/{code}")]
        public async Task<ActionResult<List<CurrencyDto>>> RefreshFromApi(string code)
        {
            var result = await _thirdPartyService.RefreshCurrencyCode(code);
            if (result == null)
                return NotFound($"Country with code '{code}' not found.");
            return Ok(result);
        }

        [HttpPut("updateCountryData/{id}")]
        public async Task<IActionResult> UpdateCountryData(int id, [FromBody] CurrencyDto updatedCountry)
        {
            var success = await _thirdPartyService.UpdateCountryById(id, updatedCountry);
            if (!success)
            {
                return NotFound($"Country with code '{id}' Not Found");
            }
            return NoContent();
        }

        [HttpDelete("deleteCountry/{id}")]
        public async Task<IActionResult> RemoveCountry(int id)
        {
            var success = await _thirdPartyService.RemoveCountryById(id);
            if (!success) return NotFound($"Country with code '{id}' Not Found");
            return NoContent();
        }
    }
}
