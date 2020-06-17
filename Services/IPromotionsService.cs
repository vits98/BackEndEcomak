using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface IPromotionsService
    {
        Task<IEnumerable<Promotion>> GetPromotionsAsync(bool showComments, string orderBy);
        Task<Promotion> GetPromotionAsync(int id, bool showComments);
        Task<Promotion> CreatePromotionAsync(Promotion newPromotion);
        Task<bool> DeletePromotionAsync(int id);
        Task<Promotion> UpdatePromotionAsync(int id, Promotion newPromotion);
    }
}
