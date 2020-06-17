using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecomak.Data.Entities;
using Ecomak.Data.Repository;
using Ecomak.Exceptions;
using Ecomak.Models;

namespace Ecomak.Services
{
    public class CommentsService : ICommentsService
    {
        private HashSet<string> allowedOrderByValues;
        private IEcomakRepository premierLeagueRepository;
        private readonly IMapper mapper;
        public CommentsService(IEcomakRepository premierLeagueRepository, IMapper mapper)
        {
            this.premierLeagueRepository = premierLeagueRepository;
            this.mapper = mapper;
            allowedOrderByValues = new HashSet<string>() { "author", "commet", "date", "id" };

        }

        public async Task<Commentary> AddCommentaryAsync(int promotionId, Commentary commentary)
        {
            if (commentary.promotionId != null && promotionId != commentary.promotionId)
            {
                throw new BadRequestOperationException("URL promotion id and Commentary.PromotionId should be equal");
            }
            commentary.promotionId = promotionId;
            var promotionEntity = await validatPromotionId(promotionId);
            var commentaryEntity = mapper.Map<CommentaryEntity>(commentary);

            premierLeagueRepository.CreateCommentary(commentaryEntity);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return mapper.Map<Commentary>(commentaryEntity);
            }
            throw new Exception("There were an error with the DB");
        }

        public async Task<Commentary> EditCommentaryAsync(int promotionId, int id, Commentary commentary)
        {

            if (commentary.id != null && commentary.id != id)
            {
                throw new InvalidOperationException("commentary URL id and commentary body id should be the same");
            }

            await ValidatePromotionAndCommentary(promotionId, id);

            commentary.promotionId = promotionId;
            var commentaryEntity = mapper.Map<CommentaryEntity>(commentary);
            premierLeagueRepository.UpdateCommentary(commentaryEntity);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return mapper.Map<Commentary>(commentaryEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<Commentary> GetCommentaryAsync(int promotionId, int id)
        {
            await ValidatePromotionAndCommentary(promotionId, id);
            var commentaryEntity = await premierLeagueRepository.GetCommentaryAsync(id);
            return mapper.Map<Commentary>(commentaryEntity);
        }

        public async Task<IEnumerable<Commentary>> GetComments(int promotionId)
        {
            string orderBy = "id";
            var orderByLower = orderBy.ToLower();
            if (!allowedOrderByValues.Contains(orderByLower))
            {
                throw new BadRequestOperationException($"invalid Order By value : {orderBy} the only allowed values are {string.Join(", ", allowedOrderByValues)}");
            }
            var commentsEntities = await premierLeagueRepository.GetCommentsAsync(promotionId, orderByLower);

            return mapper.Map<IEnumerable<Commentary>>(commentsEntities);
        }

        public async Task<bool> RemoveCommentary(int promotionId, int id)
        {
            await ValidatePromotionAndCommentary(promotionId,id);
            await premierLeagueRepository.DeleteCommentaryAsync(id);
            if (await premierLeagueRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        private async Task<PromotionEntity> validatPromotionId(int id, bool showComments = false)
        {
            var promotion = await premierLeagueRepository.GetPromotionAsync(id);
            if (promotion == null)
            {
                throw new NotFoundItemException($"cannot found promotion with id {id}");
            }

            return promotion;
        }

        private async Task<bool> ValidatePromotionAndCommentary(int promotionId, int commentaryId)
        {

            var promotion = await premierLeagueRepository.GetPromotionAsync(promotionId);
            if (promotion == null)
            {
                throw new NotFoundItemException($"cannot found promotion with id {promotionId}");
            }

            var commentary = await premierLeagueRepository.GetCommentaryAsync(commentaryId, true);
            if (commentary == null || commentary.Promotion.id != promotionId)
            {
                throw new NotFoundItemException($"Commentary not found with id {commentaryId} for promotion {promotionId}");
            }

            return true;
        }
    }
}
