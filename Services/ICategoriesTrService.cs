using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface ICategoriesTrService
    {
        Task<IEnumerable<Category>> GetCategoriesTrAsync(string orderBy, bool showProducts);
        Task<Category> GetCategoryTrAsync(int id, bool showProducts);
        Task<Category> AddCategoryTrAsync(Category Category);
        Task<Category> UpdateCategoryTrAsync(int id, Category Category);
        Task<bool> DeleteCategoryTrAsync(int id);
        Task<IEnumerable<Product>> GetAllTrsAsync(string orderBy);
    }
}
