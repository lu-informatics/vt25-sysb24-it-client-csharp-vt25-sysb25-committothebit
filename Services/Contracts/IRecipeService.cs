using System.Collections.Generic;
using System.Threading.Tasks;
using Informatics.MauiClient.Models;

namespace Appetite.MauiDbClient.Services.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<Recipe> GetRecipeAsync(int id);
        Task<Recipe> CreateRecipeAsync(Recipe recipe);
        Task<bool> UpdateRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeAsync(int id);
    }
}
