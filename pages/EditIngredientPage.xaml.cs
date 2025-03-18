using Appetite.MauiDbClient;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class EditIngredientPopup : Popup
    {
        public Ingredient Ingredient { get; private set; }

        public EditIngredientPopup(Ingredient ingredient)
        {
            InitializeComponent();
            // Save the original ingredient (ID should remain unchanged)
            Ingredient = ingredient;

            // Initialize UI with current values
            IdLabel.Text = $"ID: {ingredient.Id}";
            NameEntry.Text = ingredient.Name;
            CategoryEntry.Text = ingredient.Category;
            UnitEntry.Text = ingredient.Unit;
            DietTagEntry.Text = ingredient.DietTag;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Trim and validate inputs
            string name = NameEntry.Text?.Trim() ?? "";
            string category = CategoryEntry.Text?.Trim() ?? "";
            string unit = UnitEntry.Text?.Trim() ?? "";
            string dietTag = DietTagEntry.Text?.Trim() ?? "";
            
            // Regex pattern to allow only letters and spaces
            string lettersPattern = @"^[a-zA-Z\s]+$";

            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, lettersPattern))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid Name (letters and spaces only).", "OK");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(category) || !Regex.IsMatch(category, lettersPattern))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid Category (letters and spaces only).", "OK");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(unit) || !Regex.IsMatch(unit, lettersPattern))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid Unit (letters and spaces only).", "OK");
                return;
            }

            // Validate Diet Tag field to ensure it's one of the allowed values.
            var validDietTags = new List<string> { "Non-Vegetarian", "Pescatarian", "Vegetarian", "Vegan" };
            if (!validDietTags.Contains(dietTag))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid diet tag (Non-Vegetarian, Pescatarian, Vegetarian, or Vegan).", "OK");
                return;
            }

            // Create a new record with updated values while preserving the Id
            Ingredient = Ingredient with
            {
                Name = name,
                Category = category,
                Unit = unit,
                DietTag = dietTag
            };

            // Close the popup and return the updated ingredient
            Close(Ingredient);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            // Close the popup without returning a value
            Close(null);
        }
    }
}
