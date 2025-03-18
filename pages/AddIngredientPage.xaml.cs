using Microsoft.Maui.Controls;
using Appetite.MauiDbClient.Services.Contracts;
using Appetite.MauiDbClient.Services;
using System.Threading.Tasks;
using Appetite.MauiDbClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MauiApp1
{
    [QueryProperty(nameof(IngredientId), "ingredientId")]
    public partial class AddIngredientPage : ContentPage
    {
        private readonly IIngredientService _ingredientService;

        // This property is automatically set when navigating with "?ingredientId=123"
        public int? IngredientId { get; set; }

        public AddIngredientPage()
        {
            InitializeComponent();
            // In a DI scenario, you might resolve the service instead of creating it directly.
            _ingredientService = new IngredientService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // If IngredientId is set, we’re editing an existing ingredient
            if (IngredientId.HasValue)
            {
                await LoadIngredientAsync(IngredientId.Value);
            }
            // Otherwise, we are creating a new ingredient
        }

        private async Task LoadIngredientAsync(int id)
        {
            var ingredient = await _ingredientService.GetIngredientAsync(id);
            if (ingredient != null)
            {
                // Populate UI fields
                NameEntry.Text = ingredient.Name;
                CategoryEntry.Text = ingredient.Category;
                UnitEntry.Text = ingredient.Unit;
                DietTagEntry.Text = ingredient.DietTag;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validate Name, Category, and Unit entries to ensure they contain only letters and spaces
            string name = NameEntry.Text?.Trim() ?? "";
            string category = CategoryEntry.Text?.Trim() ?? "";
            string unit = UnitEntry.Text?.Trim() ?? "";
            
            string lettersPattern = @"^[a-zA-Z\s]+$";

            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, lettersPattern))
            {
                await DisplayAlert("Error", "Please enter a valid Name (letters and spaces only).", "OK");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(category) || !Regex.IsMatch(category, lettersPattern))
            {
                await DisplayAlert("Error", "Please enter a valid Category (letters and spaces only).", "OK");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(unit) || !Regex.IsMatch(unit, lettersPattern))
            {
                await DisplayAlert("Error", "Please enter a valid Unit (letters and spaces only).", "OK");
                return;
            }

            // Validate Diet Tag field to ensure it's one of the allowed values.
            var validDietTags = new List<string> { "Non-Vegetarian", "Pescatarian", "Vegetarian", "Vegan" };
            string dietTag = DietTagEntry.Text?.Trim() ?? "";

            if (!validDietTags.Contains(dietTag))
            {
                await DisplayAlert("Error", "Please enter a valid diet tag (Non-Vegetarian, Pescatarian, Vegetarian, or Vegan).", "OK");
                return;
            }

            // Create or update the ingredient (ID is not editable)
            var ingredient = new Ingredient
            {
                Id = IngredientId ?? 0, // For a new ingredient, this might be 0
                Name = name,
                Category = category,
                Unit = unit,
                DietTag = dietTag
            };

            if (IngredientId.HasValue)
            {
                // Update existing ingredient
                var success = await _ingredientService.UpdateIngredientAsync(ingredient);
                if (!success)
                {
                    await DisplayAlert("Error", "Could not update ingredient.", "OK");
                    return;
                }
            }
            else
            {
                // Create a new ingredient
                await _ingredientService.CreateIngredientAsync(ingredient);
            }

            // Go back to the previous page
            await Shell.Current.GoToAsync("..");
        }
    }
}
