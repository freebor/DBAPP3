using DBAPP3.Models.DTOs;
using DBAPP3.Repository;

namespace DBAPP3.DataSeeding
{
    public static class CountrySeeding
    {
        public static async Task SeedAsync(ICountryRepository repo)
        {
            //var existing = await repo.GetAllCountriesAsync();
            //if (existing.Count > 0) return;

            var countries = new List<CurrencyDto>
            {
                new() { Name = "United States", Currency = "USD", Capital = "Washington, D.C.", Region = "Americas", Population = 331000000, Flag = "https://flagcdn.com/us.png", CreatedDate = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                new() { Name = "United Kingdom", Currency = "GBP", Capital = "London", Region = "Europe", Population = 67000000, Flag = "https://flagcdn.com/gb.png", CreatedDate = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                new() { Name = "Japan", Currency = "JPY", Capital = "Tokyo", Region = "Asia", Population = 125000000, Flag = "https://flagcdn.com/jp.png", CreatedDate = DateTime.UtcNow, LastUpdated = DateTime.UtcNow }
            };

            //foreach (var c in countries)
            //{
            //    await repo.AddCountryAsync(c);
            //}

            var existing = await repo.GetAllCountriesAsync();

            foreach (var country in countries)
            {
                if (!existing.Any(c => c.Name == country.Name))
                {
                    await repo.AddCountryAsync(country);
                }
            }
        }
    }

}
