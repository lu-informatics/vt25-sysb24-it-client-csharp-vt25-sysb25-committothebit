using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Appetite.MauiDbClient.Services.Contracts;
using CommunityToolkit.Maui.Views;
using MauiApp1;
using Microsoft.Maui.Controls;  // För Command och Shell-navigering

namespace Appetite.MauiDbClient.ViewModels
{
    public class IngredientViewModel
    {
        private readonly IIngredientService _ingredientService;
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public IngredientViewModel(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
            Ingredients = new ObservableCollection<Ingredient>();
            RefreshCommand = new Command(async () => await LoadIngredientsAsync());
            AddCommand = new Command(OnAddIngredient);
            DeleteCommand = new Command<Ingredient>(async (ingredient) => await OnDeleteIngredient(ingredient));
            EditCommand = new Command<Ingredient>(async (ingredient) => await OnEditIngredient(ingredient));
        }

        private async Task LoadIngredientsAsync()
        {
            var ingredients = await _ingredientService.GetIngredientsAsync();
            Ingredients.Clear();
            foreach (var ingredient in ingredients)
            {
                Ingredients.Add(ingredient);
            }
        }

        private async void OnAddIngredient()
{
    // Navigate using the route name
    await Shell.Current.GoToAsync($"{nameof(AddIngredientPage)}");
}
        private async Task OnDeleteIngredient(Ingredient ingredient)
{
    if (ingredient == null) return;

    // Show confirmation
    bool confirm = await Application.Current.MainPage.DisplayAlert(
        "Confirm Delete", 
        $"Are you sure you want to delete {ingredient.Name}?", 
        "Yes", 
        "No"
    );

    if (!confirm) 
        return; // User clicked "No", so just return.

    // Proceed to delete
    var success = await _ingredientService.DeleteIngredientAsync(ingredient.Id);
    if (success)
    {
        Ingredients.Remove(ingredient);
    }
    else
    {
        await Application.Current.MainPage.DisplayAlert("Error", "Could not delete ingredient.", "OK");
    }
}

private async Task OnEditIngredient(Ingredient ingredient)
{
    if (ingredient == null) return;

    // Create the popup with the current ingredient
    var popup = new EditIngredientPopup(ingredient);
    
    // Show the popup. (Requires CommunityToolkit.Maui to be set up.)
    var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    
    // If the user saved changes, result will be an updated Ingredient record
    if (result is Ingredient updatedIngredient)
    {
        // Update via the service
        var success = await _ingredientService.UpdateIngredientAsync(updatedIngredient);
        if (success)
        {
            // Option 1: Refresh the ingredients list, or
            // Option 2: Update the item in the ObservableCollection
            await LoadIngredientsAsync(); 
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Could not update ingredient.", "OK");
        }
    }
}
    }
}
