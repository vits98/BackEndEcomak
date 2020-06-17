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
    [Route("api/categoriesTr/{categoryId:int}/trs")]

    public class TrsController : ControllerBase
    {
        private ITrService trsService;

        public TrsController(ITrService trsService)
        {
            this.trsService = trsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tr>>> GetTrs(int categoryId)
        {
            try
            {
                var Trs = await trsService.GetTrsAsync(categoryId);
                return Ok(Trs);
            }
            catch (BadRequestOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something bad happend");
            }
        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Tr>> GetTr(int id, int categoryId)
        {
            try
            {
                var Tr = await this.trsService.GetTrAsync(id, categoryId);
                return Ok(Tr);
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something bad happend");
            }
        }

        [HttpGet("by_id/{Id:int}")]
        public async Task<ActionResult<Tr>> GetTrById(int id)
        {
            try
            {
                var Tr = await this.trsService.GetTrByIdTr(id);
                return Ok(Tr);
            }
            catch (NotFoundItemException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "something bad happend");
            }
        }

        [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult<Tr>> UpdateTr(int id, int categoryId, [FromBody]Tr Tr)
        {
            try
            {
                return Ok(await this.trsService.UpdateTrAsync(id, categoryId, Tr));
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
        [HttpPost]
        public async Task<ActionResult<Tr>> PostTr(int categoryId, [FromBody] Tr tr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postedTr = await this.trsService.CreateTr(categoryId, tr);
            return Created($"/api/categoriesTr/{categoryId}/trs/{postedTr.IdTr}", postedTr);
        }
        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteTr(int id, int categoryId)
        {
            try
            {
                return Ok(await this.trsService.DeleteTrAsync(categoryId, id));
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


    }
}
