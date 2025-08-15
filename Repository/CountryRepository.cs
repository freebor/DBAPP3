using DBAPP3.Models.DTOs;
using System.Data;
using System.Data.SqlClient;

namespace DBAPP3.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CountryRepository> _logger;

        public CountryRepository(IConfiguration configuration, ILogger<CountryRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        public async Task<List<CurrencyDto>> GetAllCountriesAsync()
        {
            var countries = new List<CurrencyDto>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        SELECT Id, Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated 
                        FROM Countries h                        ORDER BY Name";

                using var command = new SqlCommand(sql, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    countries.Add(MapReaderToCountry(reader));
                }

                _logger.LogInformation("Retrieved {Count} countries from database", countries.Count);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving all countries");
                throw new InvalidOperationException("Database error occurred while retrieving countries", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving all countries");
                throw;
            }

            return countries;
        }

        public async Task<CurrencyDto?> GetCountryByCurrencyAsync(string code)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        SELECT Id, Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated 
                        FROM Countries 
                        WHERE Currency = @Currency";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Currency", SqlDbType.NVarChar, 10).Value = code;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var country = MapReaderToCountry(reader);
                    _logger.LogInformation("Retrieved country {Currency} from database", code);
                    return country;
                }

                _logger.LogInformation("Country with code {Currency} not found in database", code);
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving country with code {Currency}", code);
                throw new InvalidOperationException($"Database error occurred while retrieving country with Currency {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving country with code {Code}", code);
                throw;
            }
        }
        public async Task<CurrencyDto?> GetCountryByCountryName(string name)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        SELECT Id, Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated 
                        FROM Countries 
                        WHERE Name = @Name";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 10).Value = name;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var country = MapReaderToCountry(reader);
                    _logger.LogInformation("Retrieved country {Currency} from database", name);
                    return country;
                }

                _logger.LogInformation("Country with code {Currency} not found in database", name);
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving country with country name {Name}", name);
                throw new InvalidOperationException($"Database error occurred while retrieving country with Name {name}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving country with Name {NAme}", name);
                throw;
            }
        }

        public async Task<CurrencyDto?> GetCountryByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        SELECT Id, Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated 
                        FROM Countries 
                        WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var country = MapReaderToCountry(reader);
                    _logger.LogInformation("Retrieved country with ID {Id} from database", id);
                    return country;
                }

                _logger.LogInformation("Country with ID {Id} not found in database", id);
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving country with ID {Id}", id);
                throw new InvalidOperationException($"Database error occurred while retrieving country with ID {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving country with ID {Id}", id);
                throw;
            }
        }

        public async Task<CurrencyDto> AddCountryAsync(CurrencyDto country)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        INSERT INTO Countries (Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @Currency, @Capital, @Population, @Region, @Flag, @CreatedDate, @LastUpdated)";

                using var command = new SqlCommand(sql, connection);
                AddCountryParameters(command, country);

                var newId = (int)await command.ExecuteScalarAsync();
                country.Id = newId;

                _logger.LogInformation("Added new country {Code} with ID {Id} to database", country.Currency, newId);
                return country;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding country {Code}", country.Currency);
                throw new InvalidOperationException($"Database error occurred while adding country {country.Currency}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding country {Code}", country.Currency);
                throw;
            }
        }

        public async Task<CurrencyDto?> UpdateCountryAsync(CurrencyDto country)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                        UPDATE Countries 
                        SET Name = @Name, Currency = @Currency, Capital = @Capital, 
                            Population = @Population, Region = @Region, Flag = @Flag, 
                            LastUpdated = @LastUpdated
                        WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = country.Id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = country.Name;
                command.Parameters.Add("@Currency", SqlDbType.NVarChar, 10).Value = (object?)country.Currency ?? DBNull.Value;
                command.Parameters.Add("@Capital", SqlDbType.NVarChar, 100).Value = (object?)country.Capital ?? DBNull.Value;
                command.Parameters.Add("@Population", SqlDbType.BigInt).Value = (object?)country.Population ?? DBNull.Value;
                command.Parameters.Add("@Region", SqlDbType.NVarChar, 50).Value = (object?)country.Region ?? DBNull.Value;
                command.Parameters.Add("@Flag", SqlDbType.NVarChar, 500).Value = (object?)country.Flag ?? DBNull.Value;
                command.Parameters.Add("@LastUpdated", SqlDbType.DateTime2).Value = DateTime.Now;

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    country.LastUpdated = DateTime.Now;
                    _logger.LogInformation("Updated country with ID {Id} in database", country.Id);
                    return country;
                }

                _logger.LogWarning("No rows were updated for country with ID {Id}", country.Id);
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating country with ID {Id}", country.Id);
                throw new InvalidOperationException($"Database error occurred while updating country with ID {country.Id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating country with ID {Id}", country.Id);
                throw;
            }
        }

        public async Task<List<CurrencyDto>> SearchCountriesAsync(string searchTerm)
        {
            var countries = new List<CurrencyDto>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                SELECT Id, Name, Currency, Capital, Population, Region, Flag, CreatedDate, LastUpdated 
                FROM Countries
                WHERE Name LIKE @Search OR Capital LIKE @Search OR Currency LIKE @Search";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = $"%{searchTerm}%";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    countries.Add(MapReaderToCountry(reader));
                }

                _logger.LogInformation("Search returned {Count} countries for term '{Term}'", countries.Count, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching countries with term '{Term}'", searchTerm);
                throw;
            }

            return countries;
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = "DELETE FROM Countries WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    _logger.LogInformation("Deleted country with ID {Id} from database", id);
                    return true;
                }

                _logger.LogWarning("No country found with ID {Id} to delete", id);
                return false;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting country with ID {Id}", id);
                throw new InvalidOperationException($"Database error occurred while deleting country with ID {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting country with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> CountryExistsAsync(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("CountryExistsAsync was called with null or empty currency code.");
                return false;
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = "SELECT COUNT(1) FROM Countries WHERE Currency = @Currency";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Currency", SqlDbType.NVarChar, 10).Value = code;

                var count = (int)await command.ExecuteScalarAsync();
                var exists = count > 0;

                _logger.LogInformation("Country existence check for {Currency}: {Exists}", code, exists);
                return exists;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while checking if country {Currency} exists", code);
                throw new InvalidOperationException($"Database error occurred while checking if country {code} exists", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while checking if country {Currency} exists", code);
                throw;
            }
        }

        private static CurrencyDto MapReaderToCountry(SqlDataReader reader)
        {
            return new CurrencyDto
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                //Code = reader.GetString("Code"),
                Currency = reader.IsDBNull("Currency") ? null : reader.GetString("Currency"),
                Capital = reader.IsDBNull("Capital") ? null : reader.GetString("Capital"),
                Population = reader.IsDBNull("Population") ? null : reader.GetInt64("Population"),
                Region = reader.IsDBNull("Region") ? null : reader.GetString("Region"),
                Flag = reader.IsDBNull("Flag") ? null : reader.GetString("Flag"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                LastUpdated = reader.GetDateTime("LastUpdated")
            };
        }

        private static void AddCountryParameters(SqlCommand command, CurrencyDto country)
        {
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = country.Name;
            command.Parameters.Add("@Currency", SqlDbType.NVarChar, 10).Value = (object?)country.Currency ?? DBNull.Value;
            command.Parameters.Add("@Capital", SqlDbType.NVarChar, 100).Value = (object?)country.Capital ?? DBNull.Value;
            command.Parameters.Add("@Population", SqlDbType.BigInt).Value = (object?)country.Population ?? DBNull.Value;
            command.Parameters.Add("@Region", SqlDbType.NVarChar, 50).Value = (object?)country.Region ?? DBNull.Value;
            command.Parameters.Add("@Flag", SqlDbType.NVarChar, 500).Value = (object?)country.Flag ?? DBNull.Value;
            command.Parameters.Add("@CreatedDate", SqlDbType.DateTime2).Value = country.CreatedDate;
            command.Parameters.Add("@LastUpdated", SqlDbType.DateTime2).Value = country.LastUpdated;
        }
    }
}
