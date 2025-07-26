//using DBAPP3.Models.DTOs;
//using DBAPP3.Services;
//using DBAPP3.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace DBAPP3.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class CurrencyController : ControllerBase
//    {

//        private readonly ILogger<CurrencyController> _logger;
//        private readonly IThirdPartyService _thirdPartyService;

//        public CurrencyController(ILogger<CurrencyController> logger, IThirdPartyService thirdPartyService)
//        {
//            _logger = logger;
//            _thirdPartyService = thirdPartyService;
//        }

//        [HttpGet(Name = "GetCurrency")]
//        public async Task<ActionResult> GetCurrency(string request)
//        {
//            if (string.IsNullOrWhiteSpace(request))
//            {
//                return BadRequest("please input a valid currency-code");
//            }
//            var result = await _thirdPartyService.GetCurrency(request);
//            return Ok(result);
//        }
//    }
//}
