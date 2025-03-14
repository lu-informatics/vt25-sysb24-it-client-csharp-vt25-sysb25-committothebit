using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Appetite.MauiDbClient.Services.Contracts;
using System.Net.Http.Json;

namespace Appetite.MauiDbClient.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5156/api/";

        public IngredientService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {
            var responseStream = await _httpClient.GetStreamAsync("ingredient");
            var ingredients = await JsonSerializer.DeserializeAsync<List<Ingredient>>(responseStream)
                              ?? new List<Ingredient>();
            return ingredients;
        }

        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            var responseStream = await _httpClient.GetStreamAsync($"ingredient/{id}");
            var ingredient = await JsonSerializer.DeserializeAsync<Ingredient>(responseStream);
            return ingredient;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            var json = JsonSerializer.Serialize(ingredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("ingredient", content);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            var createdIngredient = await JsonSerializer.DeserializeAsync<Ingredient>(responseStream);
            return createdIngredient;
        }

        public async Task<bool> UpdateIngredientAsync(Ingredient ingredient)
        {
            var json = JsonSerializer.Serialize(ingredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"ingredient/{ingredient.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteIngredientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"ingredient/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
