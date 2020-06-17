using AutoMapper;
using Ecomak.Data.Repository;
using Ecomak.Exceptions;
using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecomak.Data.Entities;

namespace Ecomak.Services
{
    public class TrsService : ITrService
    {
        private IEcomakRepository EcomakRepository;
        private readonly IMapper mapper;

        public TrsService(IEcomakRepository EcomakRepository, IMapper mapper)
        {
            this.EcomakRepository = EcomakRepository;
            this.mapper = mapper;
        }

        private HashSet<string> allowedOrderByValues = new HashSet<string>()
        {
            "Id",
            "Name",
            "Price",
        };


        private async Task ValidateCategory(int id)
        {
            var Category = await EcomakRepository.GetCategoryTrAsync(id);
            if (Category == null)
            {
                throw new NotFoundItemException($"cannot found Category with id:{id}");
            }
        }

        public async Task<IEnumerable<Tr>> GetTrsAsync(int CategoryId)
        {
            //ValidateCategory(CategoryId);
            string orderBy = "Id";
            var orderByToLower = orderBy.ToLower();
            var Trentities = await EcomakRepository.GetTrsAsync(CategoryId);
            return mapper.Map<IEnumerable<Tr>>(Trentities);
        }

        public async Task<Tr> GetTrAsync(int id, int CategoryId)
        {
            var TrEntity = await EcomakRepository.GetTrAsync(id);
            if (TrEntity == null)
            {
                throw new NotFoundItemException("Tr not found");
            }

            return mapper.Map<Tr>(TrEntity);
        }

        public async Task<Tr> CreateTr(int CategoryId, Tr Tr)
        {
            Tr.CategoryId = CategoryId;
            await ValidateCategory(CategoryId);

            var TrEntity = mapper.Map<TrEntity>(Tr);
  
            EcomakRepository.CreateTr(TrEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Tr>(TrEntity);
            }
            throw new Exception("there where and error with the DB");
        }

        public async Task<bool> DeleteTrAsync(int CategoryId, int id)
        {
            await ValidateCategory(CategoryId);
            await EcomakRepository.DeleteTrAsync(id);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return true;
            }

            return false;
        }

        public async Task<Tr> UpdateTrAsync(int id, int CategoryId, Tr Tr)
        {

            if (id != Tr.IdTr)
            {
                throw new NotFoundItemException($"not found Tr with id:{id}");
            }

            await ValidateTrUpdate(id, CategoryId, Tr);
            Tr.IdTr = id;
            Tr.CategoryId = CategoryId;
            var TrEntity = mapper.Map<TrEntity>(Tr);
            EcomakRepository.UpdateTrAsync(TrEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Tr>(TrEntity);
            }
            throw new Exception("there were an error with the BD");
        }

        private async Task ValidateTrUpdate(int id, int CategoryId, Tr EditTr)
        {
            var Category = await EcomakRepository.GetCategoryAsync(CategoryId);
            if (Category == null)
            {
                throw new NotFoundItemException($"CanNot found Category with id {CategoryId}");
            }

            var Tr = await EcomakRepository.GetTrAsync(id);
            if (Tr == null)
            {
                throw new NotFoundItemException($"CanNot found Category with id {id}");
            }
        }

        public async Task<Tr> GetTrByIdTr(int id)
        {
            var TrEntity = await EcomakRepository.GetTrAsync(id);
            if (TrEntity == null)
            {
                throw new NotFoundItemException("Tr not found");
            }
            return mapper.Map<Tr>(TrEntity);
        }
    }
}
