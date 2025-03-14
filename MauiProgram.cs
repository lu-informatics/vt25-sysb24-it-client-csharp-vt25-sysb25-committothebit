using Appetite.MauiDbClient.Services;
using Appetite.MauiDbClient.Services.Contracts;
using Appetite.MauiDbClient.ViewModels;
using CommunityToolkit.Maui;
using MauiApp1;

namespace Appetite.MauiDbClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<IIngredientService, IngredientService>();
            builder.Services.AddTransient<IRecipeService, RecipeService>();
            builder.Services.AddTransient<IngredientViewModel>();
            builder.Services.AddTransient<RecipeViewModel>();
            builder.Services.AddSingleton<IngredientPage>();
            builder.Services.AddSingleton<RecipePage>();
            builder.Services.AddTransient<AddIngredientPage>();
            builder.Services.AddTransient<AddRecipePage>();

            return builder.Build();
        }
    }
}
