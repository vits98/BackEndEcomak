using Ecomak.Exceptions;
using Ecomak.Models;
using Ecomak.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ecomak.Controllers
{
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private IQuotesService quotesService;

        public QuotesController(IQuotesService _quotesService)
        {
            this.quotesService = _quotesService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            try
            {
                return Ok(await quotesService.GetQuotesAsync());
            }
            catch (BadRequestOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something bad happened");
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuoteAsync(int id)
        {
            try
            {
                var quote = await quotesService.GetQuoteAsync(id);
                return Ok(quote);
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote([FromBody] Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createQuote = await quotesService.CreateQuote(quote);
            return Created($"/api/Quotes/{createQuote.Id}", createQuote);
        }
        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteQuote(int Id)
        {
            try
            {
                return Ok(await this.quotesService.DeleteQuoteAsync(Id));
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Quote>> PutQuote(int id, [FromBody]Quote Quote)
        {
            try
            {
                return Ok(await quotesService.UpdateQuoteAsync(id, Quote));
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
