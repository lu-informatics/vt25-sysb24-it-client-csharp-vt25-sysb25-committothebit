
using Informatics.MauiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appetite.MauiDbClient.Services.Contracts
{
    public interface IRecipeIngredientService
    {
        Task<IEnumerable<RecipeIngredient>> GetRecipeIngredientsAsync();
        Task<RecipeIngredient> GetRecipeIngredientAsync(int id);
    }
}

