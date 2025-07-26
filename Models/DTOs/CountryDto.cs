using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Country
{
    [JsonPropertyName("name")]
    public Name Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("currencies")]
    // This is a dictionary where the key is the currency code (e.g., "USD", "EUR")
    // and the value is CurrencyDetails.
    public Dictionary<string, CurrencyDetails> Currencies { get; set; }

    [JsonPropertyName("capital")]
    // Capital is typically an array of strings, as some countries have multiple capitals.
    public List<string> Capital { get; set; }

    [JsonPropertyName("population")]
    public long? Population { get; set; } // Nullable as population might not always be present or can be very large
}

// Represents the 'name' object within a country.
public class Name
{
    [JsonPropertyName("common")]
    public string Common { get; set; }

    [JsonPropertyName("official")]
    public string Official { get; set; }
}

// Represents the structure of a currency entry within the 'currencies' dictionary.
public class CurrencyDetails
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
}
