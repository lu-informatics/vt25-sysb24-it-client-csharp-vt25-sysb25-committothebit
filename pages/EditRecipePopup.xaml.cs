using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Informatics.MauiClient.Models;
using System;
using System.Linq;

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

        private void OnSaveClicked(object sender, EventArgs e)
        {
            int.TryParse(CookingTimeEntry.Text, out int cookingTime);
            int.TryParse(ServingsEntry.Text, out int servings);

            // Create a new record with updated values while preserving the Id
            Recipe = Recipe with
            {
                Name = NameEntry.Text,
                Data = DataEntry.Text,
                CookingTime = cookingTime,
                Servings = servings,
                DifficultyLevel = DifficultyLevelEntry.Text,
                IngredientNames = IngredientNamesEntry.Text
                                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Trim())
            };

            Close(Recipe);
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            // Close the popup without returning a value
            Close(null);
        }
    }
}
