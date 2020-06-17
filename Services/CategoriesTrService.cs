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
    public class CategoriesTrService : ICategoriesTrService
    {
        private IEcomakRepository CategoriesTrRepository;
        private readonly IMapper mapper;

        public CategoriesTrService(IEcomakRepository CategoriesTrRepository, IMapper mapper)
        {
            this.CategoriesTrRepository = CategoriesTrRepository;
            this.mapper = mapper;
        }

        private HashSet<string> allowedOrderByQueries = new HashSet<string>()
        {
            "id",
            "Id",
            "name",
            "lastname",
            "nationallity"
        };


        public async Task<IEnumerable<Category>> GetCategoriesTrAsync(string orderBy, bool showTrs)
        {
            orderBy = orderBy.ToLower();
            if (!allowedOrderByQueries.Contains(orderBy))
            {
                throw new InvalidOperationException($"Invalid \" {orderBy} \" orderBy query param. The allowed values are {string.Join(",", allowedOrderByQueries)}");
            }

            var CategoriesTrEntities = await CategoriesTrRepository.GetCategoriesTr(orderBy, showTrs);
            var res = mapper.Map<IEnumerable<Category>>(CategoriesTrEntities);
            for (int i = 0; i < res.Count(); i++)
            {
                res.ElementAt(i).CantProducts = res.ElementAt(i).products.Count();
                res.ElementAt(i).CantTrs = res.ElementAt(i).trs.Count();
                if (!showTrs)
                {
                    res.ElementAt(i).products = new List<Product>();
                    res.ElementAt(i).trs = new List<Tr>();
                }
            }
            return res;
        }

        public async Task<Category> GetCategoryTrAsync(int id, bool showProducts)
        {
            var CategoryEntity = await CategoriesTrRepository.GetCategoryTrAsync(id, showProducts);

            if (CategoryEntity == null)
            {
                throw new NotFoundItemException("Category not found");
            }

            return mapper.Map<Category>(CategoryEntity);
        }


        public async Task<IEnumerable<Product>> GetAllTrsAsync(string orderBy)
        {

            if (!allowedOrderByQueries.Contains(orderBy))
            {
                throw new InvalidOperationException($"Invalid \" {orderBy} \" orderBy query param. The allowed values are {string.Join(",", allowedOrderByQueries)}");
            }

            var ProductsEntities = await CategoriesTrRepository.GetAllTrs(orderBy);
            return mapper.Map<IEnumerable<Product>>(ProductsEntities);
        }
        public async Task<Category> AddCategoryTrAsync(Category Category)
        {
            var CategoryEntity = mapper.Map<CategoryEntity>(Category);

            CategoriesTrRepository.CreateCategoryTr(CategoryEntity);
            if (await CategoriesTrRepository.SaveChangesAsync())
            {
                return mapper.Map<Category>(CategoryEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<Category> UpdateCategoryTrAsync(int id, Category Category)
        {
            if (id != Category.Id)
            {
                throw new InvalidOperationException("URL id needs to be the same as Category id");
            }
            await ValidateCategoryTr(id);

            Category.Id = id;
            var CategoryEntity = mapper.Map<CategoryEntity>(Category);
            CategoriesTrRepository.UpdateCategoryTr(CategoryEntity);
            if (await CategoriesTrRepository.SaveChangesAsync())
            {
                return mapper.Map<Category>(CategoryEntity);
            }

            throw new Exception("There were an error with the DB");


        }

        public async Task<bool> DeleteCategoryTrAsync(int id)
        {
            await ValidateCategoryTr(id);
            await CategoriesTrRepository.DeleteCategoryTrAsync(id);
            if (await CategoriesTrRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        private async Task ValidateCategoryTr(int id)
        {
            var Category = await CategoriesTrRepository.GetCategoryTrAsync(id);
            if (Category == null)
            {
                throw new NotFoundItemException("invalid Category to delete");
            }
            CategoriesTrRepository.DetachEntity(Category);
        }
    }
}
