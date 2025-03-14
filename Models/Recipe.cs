using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Informatics.MauiClient.Models
{
    public record class Recipe
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("data")]
        public string? Data { get; init; }

        [JsonPropertyName("cookingTime")]
        public int CookingTime { get; init; }

        [JsonPropertyName("servings")]
        public int Servings { get; init; }

        [JsonPropertyName("difficultyLevel")]
        public string? DifficultyLevel { get; init; }

        // Ändrat från RecipeIngredients till IngredientNames (som matchar servern)
        [JsonPropertyName("ingredientNames")]
        public IEnumerable<string>? IngredientNames { get; init; }
    }
}
