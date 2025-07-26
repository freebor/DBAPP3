using System.Text.Json.Serialization;

namespace DBAPP3.Models.DTOs
{
    // The JSON response is an array of country objects, so you'll deserialize into a List<Country>.
    // Example usage: List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonString);

    //public class CurrencyDto
    //{
    //    [JsonPropertyName("name")]
    //    public Name Name { get; set; }

    //    //[JsonPropertyName("tld")]
    //    //public List<string> Tld { get; set; }

    //    //[JsonPropertyName("cca2")]
    //    //public string Cca2 { get; set; }

    //    //[JsonPropertyName("ccn3")]
    //    //public string Ccn3 { get; set; }

    //    //[JsonPropertyName("cca3")]
    //    //public string Cca3 { get; set; }

    //    //[JsonPropertyName("cioc")]
    //    //public string Cioc { get; set; }

    //    //[JsonPropertyName("independent")]
    //    //public bool? Independent { get; set; }

    //    //[JsonPropertyName("status")]
    //    //public string Status { get; set; }

    //    //[JsonPropertyName("unMember")]
    //    //public bool UnMember { get; set; }

    //    [JsonPropertyName("currencies")]
    //    public Dictionary<string, CurrencyDetail> Currencies { get; set; }

    //    [JsonPropertyName("idd")] // New field: International Direct Dialing
    //    public Idd Idd { get; set; }

    //    [JsonPropertyName("capital")]
    //    public List<string> Capital { get; set; }

    //    [JsonPropertyName("altSpellings")]
    //    public List<string> AltSpellings { get; set; }

    //    [JsonPropertyName("region")]
    //    public string Region { get; set; }

    //    [JsonPropertyName("subregion")]
    //    public string Subregion { get; set; }

    //    [JsonPropertyName("languages")]
    //    public Dictionary<string, string> Languages { get; set; }

    //    [JsonPropertyName("latlng")]
    //    public List<double> Latlng { get; set; }

    //    [JsonPropertyName("landlocked")]
    //    public bool Landlocked { get; set; }

    //    [JsonPropertyName("borders")]
    //    public List<string> Borders { get; set; }

    //    [JsonPropertyName("area")]
    //    public double Area { get; set; }

    //    [JsonPropertyName("demonyms")]
    //    public Dictionary<string, Demonym> Demonyms { get; set; }

    //    [JsonPropertyName("translations")] // New field: Translations of the country name
    //    public Dictionary<string, TranslationDetail> Translations { get; set; }

    //    [JsonPropertyName("flag")]
    //    public string FlagEmoji { get; set; }

    //    [JsonPropertyName("maps")]
    //    public Maps Maps { get; set; }

    //    [JsonPropertyName("population")]
    //    public long Population { get; set; }

    //    [JsonPropertyName("gini")]
    //    public Dictionary<string, double> Gini { get; set; }

    //    [JsonPropertyName("fifa")]
    //    public string Fifa { get; set; }

    //    [JsonPropertyName("car")]
    //    public Car Car { get; set; }

    //    [JsonPropertyName("timezones")]
    //    public List<string> Timezones { get; set; }

    //    [JsonPropertyName("continents")]
    //    public List<string> Continents { get; set; }

    //    [JsonPropertyName("flags")]
    //    public Flags Flags { get; set; }

    //    [JsonPropertyName("coatOfArms")]
    //    public CoatOfArms CoatOfArms { get; set; }

    //    [JsonPropertyName("startOfWeek")]
    //    public string StartOfWeek { get; set; }

    //    [JsonPropertyName("capitalInfo")]
    //    public CapitalInfo CapitalInfo { get; set; }

    //    [JsonPropertyName("postalCode")]
    //    public PostalCode PostalCode { get; set; }
    //}

    //public class Name
    //{
    //    [JsonPropertyName("common")]
    //    public string Common { get; set; }

    //    [JsonPropertyName("official")]
    //    public string Official { get; set; }

    //    [JsonPropertyName("nativeName")]
    //    public Dictionary<string, NativeNameDetail> NativeName { get; set; }
    //}

    //public class NativeNameDetail
    //{
    //    [JsonPropertyName("official")]
    //    public string Official { get; set; }

    //    [JsonPropertyName("common")]
    //    public string Common { get; set; }
    //}

    //public class CurrencyDetail
    //{
    //    [JsonPropertyName("name")]
    //    public string Name { get; set; }

    //    [JsonPropertyName("symbol")]
    //    public string Symbol { get; set; }
    //}

    //public class Idd // New class for International Direct Dialing
    //{
    //    [JsonPropertyName("root")]
    //    public string Root { get; set; }

    //    [JsonPropertyName("suffixes")]
    //    public List<string> Suffixes { get; set; }
    //}

    //public class Demonym
    //{
    //    [JsonPropertyName("f")]
    //    public string Female { get; set; }

    //    [JsonPropertyName("m")]
    //    public string Male { get; set; }
    //}

    //public class TranslationDetail // New class for country name translations
    //{
    //    [JsonPropertyName("official")]
    //    public string Official { get; set; }

    //    [JsonPropertyName("common")]
    //    public string Common { get; set; }
    //}

    //public class Maps
    //{
    //    [JsonPropertyName("googleMaps")]
    //    public string GoogleMaps { get; set; }

    //    [JsonPropertyName("openStreetMaps")]
    //    public string OpenStreetMaps { get; set; }
    //}

    //public class Car
    //{
    //    [JsonPropertyName("signs")]
    //    public List<string> Signs { get; set; }

    //    [JsonPropertyName("side")]
    //    public string Side { get; set; }
    //}

    //public class Flags
    //{
    //    [JsonPropertyName("png")]
    //    public string Png { get; set; }

    //    [JsonPropertyName("svg")]
    //    public string Svg { get; set; }

    //    [JsonPropertyName("alt")]
    //    public string Alt { get; set; }
    //}

    //public class CoatOfArms
    //{
    //    [JsonPropertyName("png")]
    //    public string Png { get; set; }

    //    [JsonPropertyName("svg")]
    //    public string Svg { get; set; }
    //}

    //public class CapitalInfo
    //{
    //    [JsonPropertyName("latlng")]
    //    public List<double> Latlng { get; set; }
    //}

    //public class PostalCode
    //{
    //    [JsonPropertyName("format")]
    //    public string Format { get; set; }

    //    [JsonPropertyName("regex")]
    //    public string Regex { get; set; }
    //}






    public class CurrencyDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Currency { get; set; }
        public string? Capital { get; set; }
        public long? Population { get; set; }
        public string? Region { get; set; }
        public string? Flag { get; set; }
        //public string Code { get; set; } = "";  // NEW: Country code property
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
