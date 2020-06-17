using Ecomak.Exceptions;
using Ecomak.Models;
using Ecomak.Services;
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
    public class ImagesController : ControllerBase
    {
        private IImagesService imageService;
        public ImagesController(IImagesService _imageService)
        {
            this.imageService = _imageService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            try
            {
                var image = await this.imageService.GetImageAsyncByIdImage(id);
                return Ok(image);
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
        [HttpPost]
        public async Task<ActionResult<Image>> PostCategory([FromBody] Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postedImage = await this.imageService.CreateImage(image);
            return Created($"/api/images/{postedImage.Id}", postedImage);
        }

    }
}
