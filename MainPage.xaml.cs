using System;
using MauiApp1;
using Microsoft.Maui.Controls;

namespace Appetite.MauiDbClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }



        // Navigate to the Ingredients page.
        private async void OnIngredientsClicked(object sender, EventArgs e)
        {
            // If using dependency injection, you might resolve IngredientPage from the DI container.
            // For this example, we'll create a new instance manually.
            // NOTE: In a real DI scenario, you could use:
            // var ingredientPage = App.Current.Services.GetService<IngredientPage>();
            var ingredientPage = new IngredientPage(new ViewModels.IngredientViewModel(new Services.IngredientService()));
            await Navigation.PushAsync(ingredientPage);
        }

        // Navigate to the Recipes page.
        private async void OnRecipesClicked(object sender, EventArgs e)
        {
            // Likewise, create the RecipePage. Replace with DI resolution if available.
            var recipePage = new RecipePage(new ViewModels.RecipeViewModel(new Services.RecipeService()));
            await Navigation.PushAsync(recipePage);
        }
    }
}
