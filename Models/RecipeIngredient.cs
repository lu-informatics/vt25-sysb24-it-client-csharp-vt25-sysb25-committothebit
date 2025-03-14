using System.Text.Json.Serialization;

namespace Informatics.MauiClient.Models
{
    public record class RecipeIngredient
    {
        [JsonPropertyName("recipeId")]
        public int RecipeId { get; init; }

        [JsonPropertyName("ingredientId")]
        public int IngredientId { get; init; }

        [JsonPropertyName("amount")]
        public double Amount { get; init; }
    }
}
