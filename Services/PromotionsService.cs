using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecomak.Exceptions;
using Ecomak.Data.Entities;
using Ecomak.Data.Repository;
using Ecomak.Models;

namespace Ecomak.Services
{
    public class PromotionsService : IPromotionsService
    {
        private HashSet<string> allowedOrderByValues;
        private IEcomakRepository premierLeagueRepository;
        private readonly IMapper mapper;
        public PromotionsService(IEcomakRepository premierLeagueRepository, IMapper mapper)
        {
            this.premierLeagueRepository = premierLeagueRepository;
            this.mapper = mapper;
            allowedOrderByValues = new HashSet<string>() { "tittle", "description", "iniDate", "endDate", "id" };
        }

        public async Task<Promotion> CreatePromotionAsync(Promotion newPromotion)
        {
            var promotionEntity = mapper.Map<PromotionEntity>(newPromotion);
            premierLeagueRepository.CreatePromotion(promotionEntity);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return mapper.Map<Promotion>(promotionEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            await validatePromotion(id);
            await premierLeagueRepository.DeletePromotionAsync(id);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        public async Task<Promotion> GetPromotionAsync(int id, bool showComments)
        {
            //validatPromotionId(id);
            //var promotion = premierLeagueRepository.GetPromotion(id, showComments);
            //return promotion;
            var promotion = await premierLeagueRepository.GetPromotionAsync(id, showComments);

            if (promotion == null)
            {
                throw new NotFoundItemException("promotion not found");
            }

            return mapper.Map<Promotion>(promotion);

        }

        public async Task<IEnumerable<Promotion>> GetPromotionsAsync(bool showComments, string orderBy)
        {
            var orderByLower = orderBy.ToLower();
            if (!allowedOrderByValues.Contains(orderByLower))
            {
                throw new BadRequestOperationException($"invalid Order By value : {orderBy} the only allowed values are {string.Join(", ", allowedOrderByValues)}");
            }
            var promotionsEntities = await premierLeagueRepository.GetPromotionsAsync(showComments, orderByLower);
            return mapper.Map<IEnumerable<Promotion>>(promotionsEntities);
        }

        public async Task<Promotion> UpdatePromotionAsync(int id, Promotion promotion)
        {
            if (id != promotion.id)
            {
                throw new InvalidOperationException("URL id needs to be the same as Promotion id");
            }
            await validatePromotion(id);

            promotion.id = id;
            var promotionEntity = mapper.Map<PromotionEntity>(promotion);
            premierLeagueRepository.UpdatePromotion(promotionEntity);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return mapper.Map<Promotion>(promotionEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        private async Task<PromotionEntity> validatePromotion(int id, bool showComments = false)
        {
            var promotion = await premierLeagueRepository.GetPromotionAsync(id);
            if (promotion == null)
            {
                throw new NotFoundItemException($"cannot found promotion with id {id}");
            }

            return promotion;
        }

       
    }
}
