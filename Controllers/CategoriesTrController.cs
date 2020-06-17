using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecomak.Exceptions;
using Ecomak.Models;
using Ecomak.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ecomak.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesTrController : ControllerBase
    {
        private ICategoriesTrService categoriesTrService;

        public CategoriesTrController(ICategoriesTrService categoriesTrService)
        {
            this.categoriesTrService = categoriesTrService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryTr(string orderBy = "Id", bool showTr = false)
        {
            try
            {
                return Ok(await categoriesTrService.GetCategoriesTrAsync(orderBy, showTr));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened: {ex.Message}");

            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoriesTr(int id, bool showTr = false)
        {
            try
            {
                var product = await this.categoriesTrService.GetCategoryTrAsync(id, showTr);
                return Ok(product);
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


        /*[HttpGet("trabajos-realizados")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts(string orderBy = "Id")
        {
            try
            {
                return Ok(await categoriesService.GetAllProductsAsync(orderBy));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Something bad happened: {ex.Message}");

            }
        }*/
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody]Category Category)
        {
            try
            {
                var ProductUpDated = await this.categoriesTrService.UpdateCategoryTrAsync(id, Category);
                return Ok(ProductUpDated);
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
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postedCategory = await this.categoriesTrService.AddCategoryTrAsync(category);
            return Created($"/api/categories/{postedCategory.Id}", postedCategory);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            try
            {
                return Ok(await this.categoriesTrService.DeleteCategoryTrAsync(id));
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
