using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ecomak.Data.Entities;
using Ecomak.Models;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;

namespace Ecomak.Data.Repository
{
    public class EcomakRepository : IEcomakRepository
    {
        private EcomakDbContext EcomakDbContext;
        private List<Product> products = new List<Product>();
        private List<Commentary> comments = new List<Commentary>();

        public EcomakRepository(EcomakDbContext EcomakDbContext)
        {
            this.EcomakDbContext = EcomakDbContext;

        }
        //Products
        public void CreateProduct(ProductEntity product)
        {
            EcomakDbContext.Entry(product.Category).State = EntityState.Unchanged;
            EcomakDbContext.Products.Add(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var ProductToDelete = await EcomakDbContext.Products.SingleAsync(a => a.Id == id);
            EcomakDbContext.Products.Remove(ProductToDelete);
        }

        public async Task<ProductEntity> GetProductAsync(int id)
        {

            IQueryable<ProductEntity> query = EcomakDbContext.Products;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync(int categoryId)
        {
            IQueryable<ProductEntity> query = EcomakDbContext.Products.Where(x => x.Category.Id == categoryId);
            query = query.AsNoTracking().Where(x => x.Category.Id == categoryId);
            return await query.ToArrayAsync();
        }

        public void UpdateProductAsync(ProductEntity product)
        {
            EcomakDbContext.Entry(product.Category).State = EntityState.Unchanged;
            EcomakDbContext.Products.Update(product);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProducts(string orderBy)
        {
            IQueryable<ProductEntity> query = EcomakDbContext.Products;

            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(a => a.Id);
                    break;
                case "name":
                    query = query.OrderBy(a => a.Type);
                    break;
                default:
                    break;
            }

            return await query.ToArrayAsync();
        }

        //Category
        public async Task<CategoryEntity> GetCategoryAsync(int id, bool showProducts = false)
        {
            IQueryable<CategoryEntity> query = EcomakDbContext.Categories;

            if (showProducts)
            {
                query = query.Include(a => a.Products);
            }
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategories(string orderBy, bool showProducts=false)
        {
            IQueryable<CategoryEntity> query = EcomakDbContext.Categories;
            query = query.Include(a => a.Products);
            query = query.Include(a => a.Trs);
            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(a => a.Id);
                    break;
                case "name":
                    query = query.OrderBy(a => a.Name);
                    break;
                default:
                    break;
            }
            var res =  await query.ToArrayAsync(); 
            return res;
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await EcomakDbContext.Categories.SingleAsync(a => a.Id == id);
            EcomakDbContext.Categories.Remove(category);
        }

        public void UpdateCategory(CategoryEntity Category)
        {
            EcomakDbContext.Categories.Update(Category);
        }

        public void CreateCategory(CategoryEntity Category)
        {
            var savedCategory = EcomakDbContext.Categories.Add(Category);
        }

        //categoria-trabajos-realizados
        public async Task<CategoryEntity> GetCategoryTrAsync(int id, bool showTrs = false)
        {
            IQueryable<CategoryEntity> query = EcomakDbContext.Categories;

            if (showTrs)
            {
                query = query.Include(a => a.Trs);
            }
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<CategoryEntity>> GetCategoriesTr(string orderBy, bool showTrs)
        {
            IQueryable<CategoryEntity> query = EcomakDbContext.Categories;
            query = query.Include(a => a.Trs);
            query = query.Include(a => a.Products);
            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(a => a.Id);
                    break;
                case "name":
                    query = query.OrderBy(a => a.Name);
                    break;
                default:
                    break;
            }
            var res = await query.ToArrayAsync();
            return res;
        }
        public async Task DeleteCategoryTrAsync(int id)
        {
            var category = await EcomakDbContext.Categories.SingleAsync(a => a.Id == id);
            EcomakDbContext.Categories.Remove(category);
        }

        public void UpdateCategoryTr(CategoryEntity Category)
        {
            EcomakDbContext.Categories.Update(Category);
        }

        public void CreateCategoryTr(CategoryEntity Category)
        {
            var savedCategory = EcomakDbContext.Categories.Add(Category);
        }
        //Promotion
        public void CreatePromotion(PromotionEntity promotion)
        {
            EcomakDbContext.Promotions.Add(promotion);
        }
        public void UpdatePromotion(PromotionEntity promotion)
        {
            EcomakDbContext.Promotions.Update(promotion);
        }


        public async Task DeletePromotionAsync(int id)
        {
            var promotionToDelete = await EcomakDbContext.Promotions.SingleAsync(a => a.id == id);
            EcomakDbContext.Promotions.Remove(promotionToDelete);
        }


        public async Task<PromotionEntity> GetPromotionAsync(int id, bool showComments)
        {
            IQueryable<PromotionEntity> query = EcomakDbContext.Promotions;

            if (showComments)
            {
                query = query.Include(a => a.Comments);
            }
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.id == id);
        }

        public async Task<IEnumerable<PromotionEntity>> GetPromotionsAsync(bool showComments, string orderBy)
        {
            IQueryable<PromotionEntity> query = EcomakDbContext.Promotions;
            if (showComments)
            {
                query = query.Include(a => a.Comments);
            }

            switch (orderBy)
            {
                case "tittle":
                    query = query.OrderByDescending(a => a.tittle);
                    break;
                case "iniDate":
                    query = query.OrderByDescending(a => a.iniDate);
                    break;
                case "endDate":
                    query = query.OrderByDescending(a => a.endDate);
                    break;
                default:
                    query = query.OrderByDescending(a => a.id);
                    break;
            }
            query = query.AsNoTracking();
            return await query.ToArrayAsync();
        }
        //comennts
        public Task<CommentaryEntity> GetCommentaryAsync(int id, bool showPromotion)
        {
            IQueryable<CommentaryEntity> query = EcomakDbContext.Comments;
            query = query.AsNoTracking();
            if (showPromotion)
            {
                query = query.Include(b => b.Promotion);
            }
            query = query.AsNoTracking();
            return query.SingleAsync(b => b.id == id);
        }

        public IEnumerable<Commentary> GetComments()
        {
            return comments;
        }

        public async Task<IEnumerable<CommentaryEntity>> GetCommentsAsync(int promotionId, string orderBy = "id")
        {
            IQueryable<CommentaryEntity> query = EcomakDbContext.Comments;
            //if (showComments)
            //{
            //    query = query.Include(a => a.Comments);
            //}

            switch (orderBy)
            {
                case "author":
                    query = query.OrderByDescending(a => a.author);
                    break;
                case "comment":
                    query = query.OrderByDescending(a => a.comment);
                    break;
                case "time":
                    query = query.OrderByDescending(a => a.time);
                    break;
                case "show":
                    query = query.OrderByDescending(a => a.show);
                    break;
                default:
                    query = query.OrderByDescending(a => a.id);
                    break;
            }
            query = query.AsNoTracking().Where(b => b.Promotion.id == promotionId);
            return await query.ToArrayAsync();
        }




        public void CreateCommentary(CommentaryEntity commentary)
        {
            EcomakDbContext.Entry(commentary.Promotion).State = EntityState.Unchanged;
            EcomakDbContext.Comments.Add(commentary);
        }
        public async Task DeleteCommentaryAsync(int id)
        {
            var commentaryToDelete = await EcomakDbContext.Comments.SingleAsync(a => a.id == id);
            EcomakDbContext.Comments.Remove(commentaryToDelete);
        }

        public void UpdateCommentary(CommentaryEntity commentary)
        {
            EcomakDbContext.Entry(commentary.Promotion).State = EntityState.Unchanged;
            EcomakDbContext.Comments.Update(commentary);
        }
        public void DetachEntity<T>(T entity) where T : class
        {
            EcomakDbContext.Entry(entity).State = EntityState.Detached;
            
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await EcomakDbContext.SaveChangesAsync()) > 0;
        }
        
        //suscribe
        public void CreateSubscribe(SubscribeEntity Subscribe)
        {
            EcomakDbContext.Subscribes.Add(Subscribe);
        }
        public async Task DeleteSubscribeAsync(int id)
        {
            var SubscribeToDelete = await EcomakDbContext.Subscribes.SingleAsync(a => a.Id == id);
            EcomakDbContext.Subscribes.Remove(SubscribeToDelete);
        }
        public async Task<SubscribeEntity> GetSubscribeAsync(int id)
        {
            IQueryable<SubscribeEntity> query = EcomakDbContext.Subscribes;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<SubscribeEntity>> GetSubscribesAsync(string orderBy = "id")
        {
            IQueryable<SubscribeEntity> query = EcomakDbContext.Subscribes;
            switch (orderBy)
            {
                case "Name":
                    query = query.OrderBy(a => a.Name);
                    break;
                case "Company":
                    query = query.OrderBy(a => a.Company);
                    break;
                case "Email":
                    query = query.OrderBy(a => a.Email);
                    break;
                case "Phone":
                    query = query.OrderBy(a => a.Phone);
                    break;
                case "Message":
                    query = query.OrderBy(a => a.Message);
                    break;
                default:
                    query = query.OrderBy(a => a.Id);
                    break;
            }
            query = query.AsNoTracking();
            return await query.ToArrayAsync();
        }
        public void UpdateSubscribe(SubscribeEntity Subscribe)
        {
            EcomakDbContext.Subscribes.Update(Subscribe);
        }

        public async Task<QuoteEntity> GetQuoteAsync(int id)
        {
            IQueryable<QuoteEntity> query = EcomakDbContext.Quotes;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<QuoteEntity>> GetQuotesAsync(string orderBy = "id")
        {
            IQueryable<QuoteEntity> query = EcomakDbContext.Quotes;

            query = query.Include(q => q.Tr);

            query = query.Include(q => q.Product);

            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(q => q.Id);
                    break;
                default:
                    break;
            }
            var b = await query.ToArrayAsync();
            return b;
        }

        public async Task DeleteQuoteAsync(int id)
        {
            var QuoteToDelete = await EcomakDbContext.Quotes.SingleAsync(a => a.Id == id);
            EcomakDbContext.Quotes.Remove(QuoteToDelete);
        }

        public void UpdateQuote(QuoteEntity quote)
        {
            EcomakDbContext.Quotes.Update(quote);
        }

        public void CreateQuote(QuoteEntity quote)
        {
            EcomakDbContext.Entry(quote.Product).State = EntityState.Unchanged;
            EcomakDbContext.Quotes.Add(quote);
        }

        public void CreateQuoteTR(QuoteEntity quote)
        {
            EcomakDbContext.Entry(quote.Tr).State = EntityState.Unchanged;
            EcomakDbContext.Quotes.Add(quote);
        }

        //images
        public async Task<ImageEntity> GetImageAsyncByIdImage(int id)
        {
            IQueryable<ImageEntity> query = EcomakDbContext.Images;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.Id == id);
        }


        public void CreateImage(ImageEntity image)
        {
            EcomakDbContext.Images.Add(image);
        }

        public void UpdateImage(ImageEntity imagee)
        {

            EcomakDbContext.Images.Update(imagee);
        }

        public async Task DeleteImageAsync(int id)
        {

            var ImagesToDelete = await EcomakDbContext.Images.SingleAsync(a => a.Id == id);
            EcomakDbContext.Images.Remove(ImagesToDelete);
        }
        //Trabajos-realizados
        public async  Task<IEnumerable<TrEntity>> GetTrsAsync(int categoryId)
        {
            IQueryable<TrEntity> query = EcomakDbContext.Trs.Where(x => x.Category.Id == categoryId);
            query = query.AsNoTracking().Where(x => x.Category.Id == categoryId);
            return await query.ToArrayAsync();
        }

        public async Task<TrEntity> GetTrAsync(int id)
        {
            IQueryable<TrEntity> query = EcomakDbContext.Trs;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(a => a.IdTr == id);
        }

        public void UpdateTrAsync(TrEntity tr)
        {
            EcomakDbContext.Entry(tr.Category).State = EntityState.Unchanged;
            EcomakDbContext.Trs.Update(tr);
        }

        public void CreateTr(TrEntity tr)
        {
            EcomakDbContext.Entry(tr.Category).State = EntityState.Unchanged;
            EcomakDbContext.Trs.Add(tr);
        }

        public async Task DeleteTrAsync(int id)
        {
            var TrToDelete = await EcomakDbContext.Trs.SingleAsync(a => a.IdTr == id);
            EcomakDbContext.Trs.Remove(TrToDelete);
        }

        public async Task<IEnumerable<TrEntity>> GetAllTrs(string orderBy = "id")
        {
            IQueryable<TrEntity> query = EcomakDbContext.Trs;

            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(a => a.IdTr);
                    break;
                case "name":
                    query = query.OrderBy(a => a.TypeTr);
                    break;
                default:
                    break;
            }

            return await query.ToArrayAsync();
        }
    }
}
