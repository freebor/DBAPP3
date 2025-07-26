using DBAPP3.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace DBAPP3.Services
{
    public interface IThirdPartyService
    {
        Task<List<CurrencyDto>> GetCountryName();
        Task<List<CurrencyDto>> GetCurrency(string currencyCode);
        Task<List<CurrencyDto>> GetCountryByName(string countryName);
        Task<ActionResult<List<CurrencyDto>>> RefreshCurrencyCode(string code);
        Task<bool> UpdateCountryById(int id, CurrencyDto updated);
        Task<bool> RemoveCountryById(int id);
    }
}
