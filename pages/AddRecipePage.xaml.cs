using Microsoft.Maui.Controls;
using Appetite.MauiDbClient.Services.Contracts;
using Appetite.MauiDbClient.Services;
using Informatics.MauiClient.Models;
using System.Threading.Tasks;

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
            int.TryParse(CookingTimeEntry.Text, out int cookingTime);
            int.TryParse(ServingsEntry.Text, out int servings);

            // Create or update the recipe (ID is not editable)
            var recipe = new Recipe
            {
                Id = RecipeId ?? 0, // For a new recipe, this might be 0
                Name = NameEntry.Text,
                Data = DataEntry.Text,
                CookingTime = cookingTime,
                Servings = servings,
                DifficultyLevel = DifficultyLevelEntry.Text,
                IngredientNames = IngredientNamesEntry.Text?.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
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

            // Navigate back to the previous page
            await Shell.Current.GoToAsync("..");
        }
    }
}
