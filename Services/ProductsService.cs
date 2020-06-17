using AutoMapper;
using Ecomak.Data.Entities;
using Ecomak.Data.Repository;
using Ecomak.Exceptions;
using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public class ProductsService : IProductsService
    {
        private IEcomakRepository EcomakRepository;
        private readonly IMapper mapper;

        public ProductsService(IEcomakRepository EcomakRepository, IMapper mapper)
        {
            this.EcomakRepository = EcomakRepository;
            this.mapper = mapper;
        }

        private HashSet<string> allowedOrderByValues = new HashSet<string>()
        {
            "Id",
            "Type"
        };


        private async Task ValidateCategory(int id)
        {
            var Category = await EcomakRepository.GetCategoryAsync(id);
            if (Category == null)
            {
                throw new NotFoundItemException($"cannot found Category with id:{id}");
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int CategoryId)
        {
            //ValidateCategory(CategoryId);
            string orderBy = "Id";
            var orderByToLower = orderBy.ToLower();
            var Productentities = await EcomakRepository.GetProductsAsync(CategoryId);
            return mapper.Map<IEnumerable<Product>>(Productentities);
        }

        public async Task<Product> GetProductAsync(int id, int CategoryId)
        {
            var ProductEntity = await EcomakRepository.GetProductAsync(id);
            if (ProductEntity == null)
            {
                throw new NotFoundItemException("Product not found");
            }

            return mapper.Map<Product>(ProductEntity);
        }

        public async Task<Product> CreateProduct(int CategoryId, Product Product)
        {
            //if (Product.CategoryId != null && CategoryId != Product.CategoryId)
            //{
            //    throw new BadRequestOperationException("URL Category id and Product.CategoryId should be equal");
            //}

            Product.CategoryId = CategoryId;
            await ValidateCategory(CategoryId);
            var ProductEntity = mapper.Map<ProductEntity>(Product);
            // ProductEntity.Category = null;  
            EcomakRepository.CreateProduct(ProductEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Product>(ProductEntity);
            }
            throw new Exception("there where and error with the DB");
        }

        public async Task<bool> DeleteProductAsync(int CategoryId, int id)
        {
            await ValidateCategory(CategoryId);
            await EcomakRepository.DeleteProductAsync(id);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return true;
            }

            return false;
        }

        public async Task<Product> UpdateProductAsync(int id, int CategoryId, Product Product)
        {

            if (id != Product.Id)
            {
                throw new NotFoundItemException($"not found Product with id:{id}");
            }

            await ValidateProductUpdate(id, CategoryId, Product);
            Product.Id = id;
            Product.CategoryId = CategoryId;
            var ProductEntity = mapper.Map<ProductEntity>(Product);
            EcomakRepository.UpdateProductAsync(ProductEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Product>(ProductEntity);
            }
            throw new Exception("there were an error with the BD");
        }

        private async Task ValidateProductUpdate(int id, int CategoryId, Product EditProduct)
        {
            var Category = await EcomakRepository.GetCategoryAsync(CategoryId);
            if (Category == null)
            {
                throw new NotFoundItemException($"CanNot found Category with id {CategoryId}");
            }

            var Product = await EcomakRepository.GetProductAsync(id);
            if (Product == null)
            {
                throw new NotFoundItemException($"CanNot found Category with id {id}");
            }
        }

        public async Task<Product> GetProductByIdProduct(int id)
        {
            var ProductEntity = await EcomakRepository.GetProductAsync(id);
            if (ProductEntity == null)
            {
                throw new NotFoundItemException("Product not found");
            }
            return mapper.Map<Product>(ProductEntity);
        }
    }
}
