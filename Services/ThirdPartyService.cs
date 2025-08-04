using DBAPP3.Models.DTOs;
using DBAPP3.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;

namespace DBAPP3.Services
{
    public class ThirdPartyService : IThirdPartyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ICountryRepository _repository;
        private readonly ILogger<ThirdPartyService> _logger;
        private readonly IMemoryCache _cache;
        public ThirdPartyService(HttpClient httpClient, 
            IConfiguration configuration, 
            ICountryRepository countryRepository,
            ILogger<ThirdPartyService> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ThirdPartyApi:BaseUrl"];
            _repository = countryRepository;
            _logger = logger;
            _cache = cache;
        }
        public async Task<List<CurrencyDto>> GetCountryName()
        {
            if (_cache.TryGetValue("AllCountries", out List<CurrencyDto> cachedCountries))
                return cachedCountries;

            var countries = await _repository.GetAllCountriesAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set("AllCountries", countries, cacheOptions);

            return countries;

        }

        public async Task<List<CurrencyDto>> GetCurrency(string currencyCode)
        {
            string cacheKey = $"CountryByCurrency_{currencyCode.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out List<CurrencyDto> cachedCountries))
                return cachedCountries;

            var existing = await _repository.GetCountryByCurrencyAsync(currencyCode);
            if (existing != null)
                return new List<CurrencyDto> { existing };

            var json = await FetchJsonDataAsync($"currency/{currencyCode}");
            if (json == null) return new List<CurrencyDto>();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            var countries = await MapAndSaveCountriesAsync(json, currencyCode, isUpdate: false);

            _cache.Set(cacheKey, countries, cacheOptions);
            return countries;
        }

        public async Task<List<CurrencyDto>> GetCountryByName(string countryName)
        {
            string cacheKey = $"CountryName_{countryName.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out List<CurrencyDto> cachedCountries))
                return cachedCountries;

            var existing = await _repository.GetCountryByCountryName(countryName);
            if (existing != null)
                return new List<CurrencyDto> { existing };

            var json = await FetchJsonDataAsync($"name/{countryName}");
            if (json == null) return new List<CurrencyDto>();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            var countries = await MapAndSaveCountriesAsync(json, countryName, isUpdate: false, isByName: true);

            _cache.Set(cacheKey, countries, cacheOptions);
            return countries;
        }

        public async Task<List<CurrencyDto>> SearchCountry(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<CurrencyDto>();

            string cacheKey = $"SearchCountry_{searchTerm.ToLower()}";
            if (_cache.TryGetValue(cacheKey, out List<CurrencyDto> cachedCountries))
                return cachedCountries;

            var countries = await _repository.SearchCountriesAsync(searchTerm);
            if (countries == null || !countries.Any())
                return new List<CurrencyDto>();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set(cacheKey, countries, cacheOptions);
            return countries;
        }
        public async Task<ActionResult<List<CurrencyDto>>> RefreshCurrencyCode(string code)
        {
            var json = await FetchJsonDataAsync($"currency/{code}");
            if (json == null) return new List<CurrencyDto>();

            var countries = await MapAndSaveCountriesAsync(json, code, isUpdate: true);
            return countries;
        }

        public async Task<bool> UpdateCountryById(int id, CurrencyDto updated)
        {
            var country = await _repository.GetCountryByIdAsync(id);
           
            if (country == null)

                return false;
            updated.Id = id;
            updated.LastUpdated = DateTime.UtcNow;
            await _repository.UpdateCountryAsync(updated);
            return true;
        }

        public async Task<bool> RemoveCountryById(int id)
        {
            var countries = await _repository.GetAllCountriesAsync();
            //if (!country.Exists(c => c.Id == id))

            var country = countries?.FirstOrDefault(c => c.Id == id);
            if (country == null)

                return false;

            await _repository.DeleteCountryAsync(id);
            return true;
        }
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private async Task<JArray?> FetchJsonDataAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            if (!response.IsSuccessStatusCode) return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            return JArray.Parse(jsonString);
        }

        private async Task<List<CurrencyDto>> MapAndSaveCountriesAsync(JArray json, string codeOrName, bool isUpdate, bool isByName = false)
        {
            var countries = new List<CurrencyDto>();
            
            foreach (var item in json)
            {
                var currencyToken = isByName
                    ? item["currencies"]?.Children<JProperty>().FirstOrDefault()?.Value
                    : item["currencies"]?[codeOrName];

                var dto = new CurrencyDto
                {
                    Name = item["name"]?["common"]?.ToString(),
                    Capital = item["capital"]?.First?.ToString(),
                    Currency = isByName ? item["currencies"]?.Children<JProperty>().FirstOrDefault()?.Name : codeOrName,
                    Region = item["region"]?.ToString(),
                    Population = item["population"]?.Value<long>() ?? 0,
                    Flag = item["flags"]?["png"]?.ToString(),
                    CreatedDate = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };

                if (isUpdate)
                {
                    if (await _repository.CountryExistsAsync(dto.Currency))
                        await _repository.UpdateCountryAsync(dto);
                    else
                        await _repository.AddCountryAsync(dto);
                }
                else
                {
                    await _repository.AddCountryAsync(dto);
                }

                countries.Add(dto);
            }
            return countries;
        }
    }
}
