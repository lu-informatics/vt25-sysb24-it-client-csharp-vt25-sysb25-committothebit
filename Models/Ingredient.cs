using System.Text.Json.Serialization;

namespace Appetite.MauiDbClient
{
    public record class Ingredient
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("category")]
        public string Category { get; init; }

        [JsonPropertyName("unit")]
        public string Unit { get; init; }

        [JsonPropertyName("dietTag")]
        public string DietTag { get; init; }
    }
}
