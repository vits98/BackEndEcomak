using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface ITrService
    {
        Task<IEnumerable<Tr>> GetTrsAsync(int categoryId);
        Task<Tr> GetTrAsync(int id, int categoryId);
        Task<Tr> GetTrByIdTr(int id);
        Task<Tr> UpdateTrAsync(int id, int categoryid, Tr tr);
        Task<Tr> CreateTr(int categoryId, Tr tr);
        Task<bool> DeleteTrAsync(int categoryId, int id);
    }
}
