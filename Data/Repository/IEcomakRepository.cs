using AutoMapper.Configuration.Conventions;
using Ecomak.Data.Entities;
using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Repository
{
    public interface IEcomakRepository
    {
        void DetachEntity<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Products
        Task<IEnumerable<ProductEntity>> GetProductsAsync(int categoryId);
        Task<ProductEntity> GetProductAsync(int id);
        void UpdateProductAsync(ProductEntity Product);
        void CreateProduct(ProductEntity Product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductEntity>> GetAllProducts(string orderBy = "id");
        //Tr
        Task<IEnumerable<TrEntity>> GetTrsAsync(int categoryId);
        Task<TrEntity> GetTrAsync(int id);
        void UpdateTrAsync(TrEntity Product);
        void CreateTr(TrEntity Product);
        Task DeleteTrAsync(int id);
        Task<IEnumerable<TrEntity>> GetAllTrs(string orderBy = "id");

        //Category
        Task<CategoryEntity> GetCategoryAsync(int id, bool showProducts = false);
        Task<IEnumerable<CategoryEntity>> GetCategories(string orderBy = "id", bool showProducts = false);
        Task DeleteCategoryAsync(int id);
        void UpdateCategory(CategoryEntity Category);
        void CreateCategory(CategoryEntity Category);
        //category trabajos-realizados
        Task<CategoryEntity> GetCategoryTrAsync(int id, bool showTr = false);
        Task<IEnumerable<CategoryEntity>> GetCategoriesTr(string orderBy = "id", bool showTr = false);
        Task DeleteCategoryTrAsync(int id);
        void UpdateCategoryTr(CategoryEntity Category);
        void CreateCategoryTr(CategoryEntity Category);

        //promotions
        Task<PromotionEntity> GetPromotionAsync(int id, bool showComments = false);
        Task<IEnumerable<PromotionEntity>> GetPromotionsAsync(bool showComments = false, string orderBy = "id");
        Task DeletePromotionAsync(int id);
        void UpdatePromotion(PromotionEntity promotion);
        void CreatePromotion(PromotionEntity promotion);

        //Quote
        Task<QuoteEntity> GetQuoteAsync(int id);
        Task<IEnumerable<QuoteEntity>> GetQuotesAsync(string orderBy = "id");
        Task DeleteQuoteAsync(int id);
        void UpdateQuote(QuoteEntity quote);
        void CreateQuote(QuoteEntity quote);
        void CreateQuoteTR(QuoteEntity quote);

        //comments
        Task<IEnumerable<CommentaryEntity>> GetCommentsAsync(int promotionId, string orderBy = "id");
        IEnumerable<Commentary> GetComments();
        Task<CommentaryEntity> GetCommentaryAsync(int id, bool showCommentary = false);
        void CreateCommentary(CommentaryEntity commentary);
        void UpdateCommentary(CommentaryEntity commentary);
        Task DeleteCommentaryAsync(int id);

        //Subscribe

        Task<SubscribeEntity> GetSubscribeAsync(int id);
        Task<IEnumerable<SubscribeEntity>> GetSubscribesAsync(string orderBy = "id");
        Task DeleteSubscribeAsync(int id);
        void UpdateSubscribe(SubscribeEntity Subscribe);
        void CreateSubscribe(SubscribeEntity Subscribe);

        //images
        Task<ImageEntity> GetImageAsyncByIdImage(int id);
        void CreateImage(ImageEntity image);
        Task DeleteImageAsync(int id);
        void UpdateImage(ImageEntity Subscribe);
    }
}
