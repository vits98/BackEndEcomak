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
    public class QuotesService : IQuotesService
    {
        private IEcomakRepository QuotesRepository;
        private readonly IMapper mapper;

        public QuotesService(IEcomakRepository _QuotesRepository, IMapper _mapper)
        {
            this.QuotesRepository = _QuotesRepository;
            this.mapper = _mapper;
        }

        private async Task ValidateProduct(int id)
        {
            var product = await QuotesRepository.GetProductAsync(id);
            if (product == null)
            {
                throw new NotFoundItemException($"cannot found product with id:{id}");
            }
        }

        private async Task ValidateTR(int id)
        {
            var TRitem = await QuotesRepository.GetTrAsync(id);
            if (TRitem == null)
            {
                throw new NotFoundItemException($"cannot found TR with id:{id}");
            }
        }

        public async Task<Quote> CreateQuote(Quote quote)
        {
            if (quote.TRId != 0)
            {
                await ValidateTR(quote.TRId);
                var trEntity = mapper.Map<TrEntity>(await QuotesRepository.GetTrAsync(quote.TRId));
                var QuoteEntity = mapper.Map<QuoteEntity>(quote);
                QuoteEntity.Product = null;
                QuoteEntity.Tr = trEntity;
                QuotesRepository.CreateQuoteTR(QuoteEntity);

                if (await QuotesRepository.SaveChangesAsync())
                {
                    return mapper.Map<Quote>(QuoteEntity);
                }
                throw new Exception("there where and error with the DB");
            }

            else
            {
                await ValidateProduct(quote.ProductId);
                var QuoteEntity = mapper.Map<QuoteEntity>(quote);
                QuotesRepository.CreateQuote(QuoteEntity);
                if (await QuotesRepository.SaveChangesAsync())
                {
                    return mapper.Map<Quote>(QuoteEntity);
                }
                throw new Exception("there where and error with the DB");
            }
        }

        public async Task<bool> DeleteQuoteAsync(int quoteId)
        {
            await QuotesRepository.DeleteQuoteAsync(quoteId);
            if (await QuotesRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }

        public async Task<Quote> GetQuoteAsync(int quoteId)
        {
            var QuoteEntity = await QuotesRepository.GetQuoteAsync(quoteId);
            if (QuoteEntity == null)
            {
                throw new NotFoundItemException("Product not found");
            }
            return mapper.Map<Quote>(QuoteEntity);
        }

        public async Task<IEnumerable<Quote>> GetQuotesAsync()
        {
            string orderBy = "Id";
            var orderByToLower = orderBy.ToLower();
            var Quoteentities = await QuotesRepository.GetQuotesAsync(orderBy);
            var cotizaciones = mapper.Map<IEnumerable<Quote>>(Quoteentities);
            foreach (Quote coti in cotizaciones)
            {
                if ((Quoteentities.ToList().Find(q => q.Id == coti.Id).Tr) != null)
                {
                    coti.TRId = Quoteentities.ToList().Find(q => q.Id == coti.Id).Tr.IdTr;  
                }
            }
            return cotizaciones;
        }

        public async Task<Quote> UpdateQuoteAsync(int quoteId, Quote quote)
        {
            if (quoteId != quote.Id)
            {
                throw new NotFoundItemException($"not found Product with id:{quoteId}");
            }

            quote.Id = quoteId;
            var quoteEntity = mapper.Map<QuoteEntity>(quote);
            QuotesRepository.UpdateQuote(quoteEntity);
            if (await QuotesRepository.SaveChangesAsync())
            {
                return mapper.Map<Quote>(quoteEntity);
            }
            throw new Exception("there were an error with the BD");
        }
    }
}
