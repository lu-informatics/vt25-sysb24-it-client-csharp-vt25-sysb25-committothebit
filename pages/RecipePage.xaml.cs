using Appetite.MauiDbClient.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage(RecipeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is RecipeViewModel viewModel)
            {
                viewModel.RefreshCommand.Execute(null);
            }
        }
    }
}
