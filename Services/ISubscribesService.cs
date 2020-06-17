using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface ISubscribesService 
    {
        Task<IEnumerable<Subscribe>> GetSubscribesAsync(string orderBy);
        Task<Subscribe> GetSubscribeAsync(int id);
        Task<Subscribe> CreateSubscribeAsync(Subscribe newSubscribe);
        Task<bool> DeleteSubscribeAsync(int id);
        Task<Subscribe> UpdateSubscribeAsync(int id, Subscribe newSubscribe);

    }
}
