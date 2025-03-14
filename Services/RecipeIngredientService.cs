using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Appetite.MauiDbClient.Services.Contracts;
using Informatics.MauiClient.Models;

namespace Appetite.MauiDbClient.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5156/api/";

        public RecipeIngredientService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        public async Task<IEnumerable<RecipeIngredient>> GetRecipeIngredientsAsync()
        {
            var responseStream = await _httpClient.GetStreamAsync("recipeingredient");
            var recipeIngredients = await JsonSerializer.DeserializeAsync<List<RecipeIngredient>>(responseStream)
                                    ?? new List<RecipeIngredient>();
            return recipeIngredients;
        }

        public async Task<RecipeIngredient> GetRecipeIngredientAsync(int id)
        {
            var responseStream = await _httpClient.GetStreamAsync($"recipeingredients/{id}");
            var recipeIngredient = await JsonSerializer.DeserializeAsync<RecipeIngredient>(responseStream);
            return recipeIngredient;
        }
    }
}
