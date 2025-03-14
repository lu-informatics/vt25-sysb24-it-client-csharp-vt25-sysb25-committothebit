using Appetite.MauiDbClient;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

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

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Create a new record with updated values while preserving the Id
            Ingredient = Ingredient with
            {
                Name = NameEntry.Text,
                Category = CategoryEntry.Text,
                Unit = UnitEntry.Text,
                DietTag = DietTagEntry.Text
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
