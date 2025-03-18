using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Informatics.MauiClient.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class EditRecipePopup : Popup
    {
        public Recipe Recipe { get; private set; }

        public EditRecipePopup(Recipe recipe)
        {
            InitializeComponent();
            Recipe = recipe;
            IdLabel.Text = $"ID: {recipe.Id}";
            NameEntry.Text = recipe.Name;
            DataEntry.Text = recipe.Data;
            CookingTimeEntry.Text = recipe.CookingTime.ToString();
            ServingsEntry.Text = recipe.Servings.ToString();
            DifficultyLevelEntry.Text = recipe.DifficultyLevel;
            IngredientNamesEntry.Text = recipe.IngredientNames != null ? string.Join(", ", recipe.IngredientNames) : "";
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validate Cooking Time and Servings as numeric fields.
            string cookingTimeText = CookingTimeEntry.Text?.Trim() ?? "";
            string servingsText = ServingsEntry.Text?.Trim() ?? "";

            if (!int.TryParse(cookingTimeText, out int cookingTime))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid number for Cooking Time.", "OK");
                return;
            }

            if (!int.TryParse(servingsText, out int servings))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid number for Servings.", "OK");
                return;
            }

            // Validate recipe name to allow only letters and spaces.
            string recipeName = NameEntry.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(recipeName) || !Regex.IsMatch(recipeName, "^[a-zA-Z\\s]+$"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid recipe name (letters and spaces only).", "OK");
                return;
            }

            // Validate difficulty level.
            string difficulty = DifficultyLevelEntry.Text?.Trim() ?? "";
            if (!(difficulty == "Easy" || difficulty == "Medium" || difficulty == "Hard"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a valid difficulty level: Easy, Medium, or Hard.", "OK");
                return;
            }

            // Process the Data field: if it's plain text, wrap it in a JSON object.
            string dataInput = DataEntry.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(dataInput))
            {
                dataInput = "{}";
            }
            else if (!dataInput.StartsWith("{") && !dataInput.StartsWith("["))
            {
                dataInput = "{\"description\":" + JsonSerializer.Serialize(dataInput) + "}";
            }

            // Process ingredient names: split by comma and trim each entry.
            var ingredientNamesArray = IngredientNamesEntry.Text?
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var ingredientNames = ingredientNamesArray != null
                ? ingredientNamesArray.Select(x => x.Trim()).ToArray()
                : null;

            // Create a new Recipe record with updated values while preserving the Id.
            Recipe = Recipe with
            {
                Name = recipeName,
                Data = dataInput,
                CookingTime = cookingTime,
                Servings = servings,
                DifficultyLevel = difficulty,
                IngredientNames = ingredientNames
            };

            // Close the popup and return the updated recipe.
            Close(Recipe);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            // Close the popup without returning a value.
            Close(null);
        }
    }
}
