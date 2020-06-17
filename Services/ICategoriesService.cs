using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(string orderBy, bool showProducts);
        Task<Category> GetCategoryAsync(int id, bool showProducts);
        Task<Category> AddCategoryAsync(Category Category);
        Task<Category> UpdateCategoryAsync(int id, Category Category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync(string orderBy);
    }
}
