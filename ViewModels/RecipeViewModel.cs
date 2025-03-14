using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Appetite.MauiDbClient.Services.Contracts;
using Informatics.MauiClient.Models;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using MauiApp1;

namespace Appetite.MauiDbClient.ViewModels
{
    public class RecipeViewModel
    {
        private readonly IRecipeService _recipeService;
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public RecipeViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            Recipes = new ObservableCollection<Recipe>();
            RefreshCommand = new Command(async () => await LoadRecipesAsync());
            AddCommand = new Command(OnAddRecipe);
            DeleteCommand = new Command<Recipe>(async (recipe) => await OnDeleteRecipe(recipe));
            EditCommand = new Command<Recipe>(async (recipe) => await OnEditRecipe(recipe));
        }

        private async Task LoadRecipesAsync()
        {
            var recipes = await _recipeService.GetRecipesAsync();
            Recipes.Clear();
            foreach (var recipe in recipes)
            {
                Recipes.Add(recipe);
            }
        }

        private async void OnAddRecipe()
        {
            // Navigate using the route name
            await Shell.Current.GoToAsync($"{nameof(AddRecipePage)}");
        }

        private async Task OnDeleteRecipe(Recipe recipe)
        {
            if (recipe == null) return;

            // Ask for confirmation before deletion
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete", 
                $"Are you sure you want to delete {recipe.Name}?", 
                "Yes", 
                "No"
            );

            if (!confirm) return;

            var success = await _recipeService.DeleteRecipeAsync(recipe.Id);
            if (success)
            {
                Recipes.Remove(recipe);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not delete recipe.", "OK");
            }
        }

        private async Task OnEditRecipe(Recipe recipe)
        {
            if (recipe == null) return;

            // Create and show the popup with the current recipe data
            var popup = new EditRecipePopup(recipe);
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);
            
            if (result is Recipe updatedRecipe)
            {
                var success = await _recipeService.UpdateRecipeAsync(updatedRecipe);
                if (success)
                {
                    await LoadRecipesAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not update recipe.", "OK");
                }
            }
        }
    }
}
