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
    public class CategoriesController : ControllerBase
    {
        private ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory(string orderBy = "Id", bool showProduct = false)
        {
            try
            {
                var a = await categoriesService.GetCategoriesAsync(orderBy, showProduct); 
                return Ok(a);
                
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
        public async Task<ActionResult<Category>> GetCategories(int id, bool showProduct = false)
        {
            try
            {
                var product = await this.categoriesService.GetCategoryAsync(id, showProduct);
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

        [HttpGet("trabajos-realizados")]
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
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody]Category Category)
        {
            try
            {
                var ProductUpDated = await this.categoriesService.UpdateCategoryAsync(id, Category);
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

            var postedCategory = await this.categoriesService.AddCategoryAsync(category);
            return Created($"/api/categories/{postedCategory.Id}", postedCategory);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            try
            {
                return Ok(await this.categoriesService.DeleteCategoryAsync(id));
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
