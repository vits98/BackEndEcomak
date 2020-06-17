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
    public class SubscribesService : ISubscribesService
    {
        private HashSet<string> allowedOrderByValues;
        private IEcomakRepository EcomakRepository;
        private readonly IMapper mapper;
        public SubscribesService(IEcomakRepository EcomakRepository, IMapper mapper)
        {
            this.EcomakRepository = EcomakRepository;
            this.mapper = mapper;
            allowedOrderByValues = new HashSet<string>() { "name", "price", "state", "id" };
        }
     
        public async Task<Subscribe> CreateSubscribeAsync(Subscribe newSubscribe)
        {
            var SubscribeEntity = mapper.Map<SubscribeEntity>(newSubscribe);
            EcomakRepository.CreateSubscribe(SubscribeEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Subscribe>(SubscribeEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<bool> DeleteSubscribeAsync(int id)
        {
            await validateSubscribe(id);
            await EcomakRepository.DeleteSubscribeAsync(id);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        public async Task<Subscribe> GetSubscribeAsync(int id)
        {
            //validatSubscribeId(id);
            //var Subscribe = EcomakRepository.GetSubscribe(id, showBooks);
            //return Subscribe;
            var Subscribe = await EcomakRepository.GetSubscribeAsync(id);
            if (Subscribe == null)
            {
                throw new NotFoundItemException("Subscribe not found");
            }
            return mapper.Map<Subscribe>(Subscribe);
        }
        public async Task<IEnumerable<Subscribe>> GetSubscribesAsync(string orderBy)
        {
            var orderByLower = orderBy.ToLower();
            if (!allowedOrderByValues.Contains(orderByLower))
            {
                throw new BadRequestOperationException($"invalid Order By value : {orderBy} the only allowed values are {string.Join(", ", allowedOrderByValues)}");
            }
            var SubscribesEntities = await EcomakRepository.GetSubscribesAsync(orderByLower);
            return mapper.Map<IEnumerable<Subscribe>>(SubscribesEntities);
        }
        public async Task<Subscribe> UpdateSubscribeAsync(int id, Subscribe Subscribe)
        {
            if (id != Subscribe.Id)
            {
                throw new InvalidOperationException("URL id needs to be the same as Subscribe id");
            }
            await validateSubscribe(id);
            Subscribe.Id = id;
            var SubscribeEntity = mapper.Map<SubscribeEntity>(Subscribe);
            EcomakRepository.UpdateSubscribe(SubscribeEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Subscribe>(SubscribeEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        private async Task<SubscribeEntity> validateSubscribe(int id)
        {
            var Subscribe = await EcomakRepository.GetSubscribeAsync(id);
            if (Subscribe == null)
            {
                throw new NotFoundItemException($"cannot found Subscribe with id {id}");
            }

            return Subscribe;
        }
    }
}

