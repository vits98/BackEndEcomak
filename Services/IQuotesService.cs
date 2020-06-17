using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface IQuotesService
    {
        Task<IEnumerable<Quote>> GetQuotesAsync();
        Task<Quote> GetQuoteAsync(int quoteId);
        Task<Quote> UpdateQuoteAsync(int quoteId, Quote quote);
        Task<Quote> CreateQuote(Quote quote);
        Task<bool> DeleteQuoteAsync(int quoteId);
    }
}
