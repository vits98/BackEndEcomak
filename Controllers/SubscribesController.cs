using Ecomak.Exceptions;
using Ecomak.Models;
using Ecomak.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Controllers
{
    [Route("api/[controller]")]
    public class SubscribesController : ControllerBase
    {
        private ISubscribesService SubscribesService;

        public SubscribesController(ISubscribesService SubscribesService)
        {
            this.SubscribesService = SubscribesService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscribe>>> GetSubscribes(string orderBy = "id")
        {
            try
            {
                return Ok(await SubscribesService.GetSubscribesAsync(orderBy));
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
        public async Task<ActionResult<Subscribe>> GetSubscribeAsync(int id)
        {
            try
            {
                var Subscribe = await SubscribesService.GetSubscribeAsync(id);
                return Ok(Subscribe);

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
        public async Task<ActionResult<Subscribe>> PostSubscribe([FromBody] Subscribe Subscribe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSubscribe = await SubscribesService.CreateSubscribeAsync(Subscribe);
            return Created($"/api/Subscribes/{createdSubscribe.Id}", createdSubscribe);
        }

        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteSubscribe(int Id)
        {
            try
            {
                return Ok(await this.SubscribesService.DeleteSubscribeAsync(Id));
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
        public async Task<ActionResult<Subscribe>> PutSubscribe(int id, [FromBody]Subscribe Subscribe)
        {
            if (!ModelState.IsValid)
            {
                var name = ModelState[nameof(Subscribe.Name)];

                if (name != null && name.Errors.Any())
                {
                    return BadRequest(name.Errors);
                }
            }

            try
            {
                return Ok(await SubscribesService.UpdateSubscribeAsync(id, Subscribe));

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
