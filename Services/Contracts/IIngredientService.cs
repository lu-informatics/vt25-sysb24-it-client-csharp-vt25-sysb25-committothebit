using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appetite.MauiDbClient.Services.Contracts
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<Ingredient> GetIngredientAsync(int id);
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
        Task<bool> UpdateIngredientAsync(Ingredient ingredient);
        Task<bool> DeleteIngredientAsync(int id);
    }
}
