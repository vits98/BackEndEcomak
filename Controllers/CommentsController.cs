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
    [Route("api/promotions/{promotionId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private ICommentsService commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Commentary>>> getComments(int promotionId)
        {
            //try
            //{
            //    var comments = commentsService.GetComments(promotionId);
            //    return Ok(comments);
            //}
            //catch (NotFoundItemException ex)
            //{
            //    return NotFound(ex.Message);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            try
            {
                return Ok(await commentsService.GetComments(promotionId));
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
        public async Task<ActionResult<Commentary>> getCommentary(int promotionId, int id)
        {
            try
            {
                var comments = await commentsService.GetCommentaryAsync(promotionId, id);
                return Ok(comments);
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        [HttpPost()]
        public async Task<ActionResult<Commentary>> PostCommentary(int promotionId, [FromBody] Commentary commentary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var commentaryCreated = await commentsService.AddCommentaryAsync(promotionId, commentary);
                return Created($"/api/promotions/{promotionId}/comments/{commentary.id}", commentaryCreated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Commentary>> PutCommentary(int promotionId, int id, [FromBody] Commentary commentary)
        {
            try
            {
                return Ok(await commentsService.EditCommentaryAsync(promotionId, id, commentary));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteCommentary(int promotionId, int id)
        {
            try
            {
                return Ok(await this.commentsService.RemoveCommentary(promotionId,id));
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
    }
}
