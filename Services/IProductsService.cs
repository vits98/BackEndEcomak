using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int categoryId);
        Task<Product> GetProductAsync(int id, int categoryId);
        Task<Product> GetProductByIdProduct(int id);
        Task<Product> UpdateProductAsync(int id, int categoryid, Product Product);
        Task<Product> CreateProduct(int categoryId, Product Product);
        Task<bool> DeleteProductAsync(int categoryId, int id);
    }
}
