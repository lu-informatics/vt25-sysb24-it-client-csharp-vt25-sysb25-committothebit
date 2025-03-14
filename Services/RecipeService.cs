using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Appetite.MauiDbClient.Services.Contracts;
using Informatics.MauiClient.Models;

namespace Appetite.MauiDbClient.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5156/api/";

        public RecipeService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            var responseStream = await _httpClient.GetStreamAsync("recipe");
            var recipes = await JsonSerializer.DeserializeAsync<List<Recipe>>(responseStream)
                          ?? new List<Recipe>();
            return recipes;
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            var responseStream = await _httpClient.GetStreamAsync($"recipe/{id}");
            var recipe = await JsonSerializer.DeserializeAsync<Recipe>(responseStream);
            return recipe;
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            var json = JsonSerializer.Serialize(recipe);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("recipe", content);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            var createdRecipe = await JsonSerializer.DeserializeAsync<Recipe>(responseStream);
            return createdRecipe;
        }

        public async Task<bool> UpdateRecipeAsync(Recipe recipe)
        {
            var json = JsonSerializer.Serialize(recipe);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"recipe/{recipe.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"recipe/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
