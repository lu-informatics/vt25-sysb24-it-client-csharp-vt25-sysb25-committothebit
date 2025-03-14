using Appetite.MauiDbClient.ViewModels;

namespace MauiApp1
{
    public partial class IngredientPage : ContentPage
    {
        public IngredientPage(IngredientViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is IngredientViewModel viewModel)
            {
                viewModel.RefreshCommand.Execute(null);
            }
        }
    }
}
