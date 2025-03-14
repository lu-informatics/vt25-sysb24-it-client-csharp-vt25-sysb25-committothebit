using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Appetite.MauiDbClient.Services.Contracts;
using Informatics.MauiClient.Models;

namespace Appetite.MauiDbClient.ViewModels
{
    public class RecipeIngredientViewModel
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        public ObservableCollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICommand RefreshCommand { get; set; }

        public RecipeIngredientViewModel(IRecipeIngredientService recipeIngredientService)
        {
            _recipeIngredientService = recipeIngredientService;
            RecipeIngredients = new ObservableCollection<RecipeIngredient>();
            RefreshCommand = new Command(async () => await LoadRecipeIngredientsAsync());
        }

        private async Task LoadRecipeIngredientsAsync()
        {
            var recipeIngredients = await _recipeIngredientService.GetRecipeIngredientsAsync();
            RecipeIngredients.Clear();
            foreach (var item in recipeIngredients)
            {
                RecipeIngredients.Add(item);
            }
        }
    }
}
