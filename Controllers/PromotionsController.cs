using System;
using Ecomak.Exceptions;
using Ecomak.Models;
using Ecomak.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ecomak.Controllers
{
    [Route("api/[controller]")]

    public class PromotionsController : Controller
    {
        private IPromotionsService promotionsService;

        public PromotionsController(IPromotionsService promotionsService)
        {
            this.promotionsService = promotionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions(bool showComments = true, string orderBy = "id")
        {
            try
            {
                return Ok(await promotionsService.GetPromotionsAsync(showComments, orderBy));
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


        [HttpGet("{id}")]
        public async Task<ActionResult<Promotion>> GetPromotionAsync(int id, bool showComments = true)
        {
            try
            {
                var promotion = await promotionsService.GetPromotionAsync(id, showComments);
                return Ok(promotion);

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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Promotion>> PostPromotion([FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPromotion = await promotionsService.CreatePromotionAsync(promotion);
            return Created($"/api/promotions/{createdPromotion.id}", createdPromotion);
        }
        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeletePromotion(int id)
        {
            try
            {
                return Ok(await this.promotionsService.DeletePromotionAsync(id));
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
        public async Task<ActionResult<Promotion>> PutPromotion(int id, [FromBody]Promotion promotion)
        {
           

            try
            {
                return Ok(await promotionsService.UpdatePromotionAsync(id, promotion));

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