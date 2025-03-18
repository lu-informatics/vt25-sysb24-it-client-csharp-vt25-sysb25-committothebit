using Microsoft.Maui.Controls;
using Appetite.MauiDbClient.Services.Contracts;
using Appetite.MauiDbClient.Services;
using System.Threading.Tasks;
using Appetite.MauiDbClient;

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
                // Populate UI fields and show the read-only Id
                NameEntry.Text = ingredient.Name;
                CategoryEntry.Text = ingredient.Category;
                UnitEntry.Text = ingredient.Unit;
                DietTagEntry.Text = ingredient.DietTag;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Create or update the ingredient (ID is not editable)
            var ingredient = new Ingredient
            {
                Id = IngredientId ?? 0, // For a new ingredient, this might be 0
                Name = NameEntry.Text,
                Category = CategoryEntry.Text,
                Unit = UnitEntry.Text,
                DietTag = DietTagEntry.Text
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
