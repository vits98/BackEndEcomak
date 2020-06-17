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
    [Route("api/categories/{categoryId:int}/products")]

    public class ProductsController : ControllerBase
    {
        private IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int categoryId)
        {
            try
            {
                var Products = await productsService.GetProductsAsync(categoryId);
                return Ok(Products);
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
        public async Task<ActionResult<Product>> GetProduct(int id, int categoryId)
        {
            try
            {
                var Product = await this.productsService.GetProductAsync(id, categoryId);
                return Ok(Product);
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
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var Product = await this.productsService.GetProductByIdProduct(id);
                return Ok(Product);
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
        public async Task<ActionResult<Product>> UpdateProduct(int id, int categoryId, [FromBody]Product Product)
        {
            try
            {
                return Ok(await this.productsService.UpdateProductAsync(id, categoryId, Product));
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
        public async Task<ActionResult<Product>> PostProduct(int categoryId, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postedProduct = await this.productsService.CreateProduct(categoryId, product);
            return Created($"/api/categories/{categoryId}/products/{postedProduct.Id}", postedProduct);
        }
        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id, int categoryId)
        {
            try
            {
                return Ok(await this.productsService.DeleteProductAsync(categoryId, id));
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
