using MauiApp1;

namespace Appetite.MauiDbClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(IngredientPage), typeof(IngredientPage));
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            Routing.RegisterRoute(nameof(AddIngredientPage), typeof(AddIngredientPage));
            Routing.RegisterRoute(nameof(AddRecipePage), typeof(AddRecipePage));
        }
    }
}
