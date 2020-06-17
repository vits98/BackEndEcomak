using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface ICommentsService
    {
        Task<IEnumerable<Commentary>> GetComments(int promotionId);
        Task<Commentary> GetCommentaryAsync(int promotionId, int id);
        Task<Commentary> AddCommentaryAsync(int promotionId, Commentary commentary);
        Task<Commentary> EditCommentaryAsync(int promotionId, int id, Commentary commentary);
        Task<bool> RemoveCommentary(int promotionId, int id);
    }
}
