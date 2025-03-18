using Appetite.MauiDbClient.Services.Contracts;
using Appetite.MauiDbClient.Services;
using Informatics.MauiClient.Models;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text.Json;

namespace MauiApp1
{
    [QueryProperty(nameof(RecipeId), "recipeId")]
    public partial class AddRecipePage : ContentPage
    {
        private readonly IRecipeService _recipeService;

        // This property is automatically set when navigating with "?recipeId=123"
        public int? RecipeId { get; set; }

        public AddRecipePage()
        {
            InitializeComponent();
            _recipeService = new RecipeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // If RecipeId is set, we’re editing an existing recipe
            if (RecipeId.HasValue)
            {
                await LoadRecipeAsync(RecipeId.Value);
            }
        }

        private async Task LoadRecipeAsync(int id)
        {
            var recipe = await _recipeService.GetRecipeAsync(id);
            if (recipe != null)
            {
                NameEntry.Text = recipe.Name;
                DataEntry.Text = recipe.Data;
                CookingTimeEntry.Text = recipe.CookingTime.ToString();
                ServingsEntry.Text = recipe.Servings.ToString();
                DifficultyLevelEntry.Text = recipe.DifficultyLevel;
                if (recipe.IngredientNames != null)
                {
                    IngredientNamesEntry.Text = string.Join(", ", recipe.IngredientNames);
                }
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validate Cooking Time and Servings as numeric fields.
            string cookingTimeText = CookingTimeEntry.Text?.Trim() ?? "";
            string servingsText = ServingsEntry.Text?.Trim() ?? "";

            if (!int.TryParse(cookingTimeText, out int cookingTime))
            {
                await DisplayAlert("Error", "Please enter a valid number for Cooking Time.", "OK");
                return;
            }
            if (!int.TryParse(servingsText, out int servings))
            {
                await DisplayAlert("Error", "Please enter a valid number for Servings.", "OK");
                return;
            }

            // Validate recipe name (letters and spaces only).
            string recipeName = NameEntry.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(recipeName) || !Regex.IsMatch(recipeName, "^[a-zA-Z\\s]+$"))
            {
                await DisplayAlert("Error", "Please enter a valid recipe name (letters and spaces only).", "OK");
                return;
            }

            // Validate difficulty level.
            string difficulty = DifficultyLevelEntry.Text?.Trim() ?? "";
            if (!(difficulty == "Easy" || difficulty == "Medium" || difficulty == "Hard"))
            {
                await DisplayAlert("Error", "Please select a valid difficulty level: Easy, Medium, or Hard.", "OK");
                return;
            }

            // Process the Data field: wrap plain text in a JSON object if needed.
            string dataInput = DataEntry.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(dataInput))
            {
                dataInput = "{}";
            }
            else if (!dataInput.StartsWith("{") && !dataInput.StartsWith("["))
            {
                dataInput = "{\"description\":" + JsonSerializer.Serialize(dataInput) + "}";
            }

            // Process ingredient names: split by comma and trim whitespace.
            var ingredientNamesArray = IngredientNamesEntry.Text?
                .Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            var ingredientNames = ingredientNamesArray != null
                ? ingredientNamesArray.Select(s => s.Trim()).ToArray()
                : null;

            // Create or update the Recipe object.
            var recipe = new Recipe
            {
                Id = RecipeId ?? 0, // For a new recipe, this might be 0
                Name = recipeName,
                Data = dataInput,
                CookingTime = cookingTime,
                Servings = servings,
                DifficultyLevel = difficulty,
                IngredientNames = ingredientNames
            };

            if (RecipeId.HasValue)
            {
                var success = await _recipeService.UpdateRecipeAsync(recipe);
                if (!success)
                {
                    await DisplayAlert("Error", "Could not update recipe.", "OK");
                    return;
                }
            }
            else
            {
                await _recipeService.CreateRecipeAsync(recipe);
            }

            // Navigate back to the previous page.
            await Shell.Current.GoToAsync("..");
        }
    }
}
