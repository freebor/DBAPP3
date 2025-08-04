using DBAPP3.Models.DTOs;
using DBAPP3.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using DBAPP3.Services;

namespace DBAPP3.Pages
{
    public class CountriesModel : PageModel
    {
        private readonly ICountryRepository _repository;
        private readonly IThirdPartyService _thirdPartyService;

        public CountriesModel(ICountryRepository repository, IThirdPartyService thirdPartyService)
        {
            _repository = repository;
            _thirdPartyService = thirdPartyService;
        }

        public List<CurrencyDto> Countries { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Countries = await _thirdPartyService.SearchCountry(SearchTerm);
            }
            else
            {
                Countries = await _repository.GetAllCountriesAsync();
            }
        }
    }
}
