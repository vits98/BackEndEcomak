using AutoMapper;
using Ecomak.Data.Entities;
using Ecomak.Data.Repository;
using Ecomak.Exceptions;
using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public class CategoriesService : ICategoriesService
    {
        private IEcomakRepository CategoriesRepository;
        private readonly IMapper mapper;

        public CategoriesService(IEcomakRepository CategoriesRepository, IMapper mapper)
        {
            this.CategoriesRepository = CategoriesRepository;
            this.mapper = mapper;
        }

        private HashSet<string> allowedOrderByQueries = new HashSet<string>()
        {
            "id",
            "Id",
            "name"
        };


        public async Task<IEnumerable<Category>> GetCategoriesAsync(string orderBy, bool showProducts)
        {
            orderBy = orderBy.ToLower();
            if (!allowedOrderByQueries.Contains(orderBy))
            {
                throw new InvalidOperationException($"Invalid \" {orderBy} \" orderBy query param. The allowed values are {string.Join(",", allowedOrderByQueries)}");
            }

            var CategoriesEntities = await CategoriesRepository.GetCategories(orderBy, showProducts);
            var res = mapper.Map<IEnumerable<Category>>(CategoriesEntities);
            for (int i = 0; i < res.Count(); i++)
            {
                res.ElementAt(i).CantProducts= res.ElementAt(i).products.Count();
                res.ElementAt(i).CantTrs = res.ElementAt(i).trs.Count();
                if (!showProducts)
                {
                    res.ElementAt(i).products = new List<Product>();
                    res.ElementAt(i).trs = new List<Tr>();
                } 
            }
            return res;
        }

        public async Task<Category> GetCategoryAsync(int id, bool showProducts)
        {
            var CategoryEntity = await CategoriesRepository.GetCategoryAsync(id, showProducts);

            if (CategoryEntity == null)
            {
                throw new NotFoundItemException("Category not found");
            }

            return mapper.Map<Category>(CategoryEntity);
        }


        public async Task<IEnumerable<Product>> GetAllProductsAsync(string orderBy)
        {

            if (!allowedOrderByQueries.Contains(orderBy))
            {
                throw new InvalidOperationException($"Invalid \" {orderBy} \" orderBy query param. The allowed values are {string.Join(",", allowedOrderByQueries)}");
            }

            var ProductsEntities = await CategoriesRepository.GetAllProducts(orderBy);
            return mapper.Map<IEnumerable<Product>>(ProductsEntities);
        }
        public async Task<Category> AddCategoryAsync(Category Category)
        {
            var CategoryEntity = mapper.Map<CategoryEntity>(Category);

            CategoriesRepository.CreateCategory(CategoryEntity);
            if (await CategoriesRepository.SaveChangesAsync())
            {
                return mapper.Map<Category>(CategoryEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category Category)
        {
            if (id != Category.Id)
            {
                throw new InvalidOperationException("URL id needs to be the same as Category id");
            }
            await ValidateCategory(id);

            Category.Id = id;
            var CategoryEntity = mapper.Map<CategoryEntity>(Category);
            CategoriesRepository.UpdateCategory(CategoryEntity);
            if (await CategoriesRepository.SaveChangesAsync())
            {
                return mapper.Map<Category>(CategoryEntity);
            }

            throw new Exception("There were an error with the DB");


        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            await ValidateCategory(id);
            await CategoriesRepository.DeleteCategoryAsync(id);
            if (await CategoriesRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        private async Task ValidateCategory(int id)
        {
            var Category = await CategoriesRepository.GetCategoryAsync(id);
            if (Category == null)
            {
                throw new NotFoundItemException("invalid Category to delete");
            }
            CategoriesRepository.DetachEntity(Category);
        }
        
    }
}
