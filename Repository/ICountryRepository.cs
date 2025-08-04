using DBAPP3.Models.DTOs;

namespace DBAPP3.Repository
{
    public interface ICountryRepository
    {
        Task<List<CurrencyDto>> GetAllCountriesAsync();
        Task<CurrencyDto?> GetCountryByCurrencyAsync(string code);
        Task<CurrencyDto?> GetCountryByIdAsync(int id);
        Task<CurrencyDto?> GetCountryByCountryName(string name);
        Task<CurrencyDto> AddCountryAsync(CurrencyDto country);
        Task<CurrencyDto?> UpdateCountryAsync(CurrencyDto country);
        Task<List<CurrencyDto>> SearchCountriesAsync(string searchTerm);
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> CountryExistsAsync(string code);
        //Task<IEnumerable<Country>> SearchCountriesAsync(string searchTerm);
    }
}
