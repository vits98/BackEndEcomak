using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Ecomak.Models;
using Ecomak.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private IImagesService imagesService;

        public UploadImageController(IImagesService _imagesService)
        {
            this.imagesService = _imagesService;
        }
        //[Authorize]
        [HttpPost, DisableRequestSizeLimit]

        public async Task <IActionResult> Upload()
        {
            Image image = new Image();
            try
            {
                var file = Request.Form.Files[0];
                var type = Request.Form.ElementAt(0);
                image.Type = type.Value;
                var folderName = Path.Combine("StaticFiles", "Images",image.Type);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                

                if (file.Length > 0)
                {
                    image.Name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    image.Origin = pathToSave;
                    var postedImage = await this.imagesService.CreateImage(image);

                    var fileName = postedImage.Id.ToString() +"-"+ image.Name;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    postedImage.Name = fileName;
                    postedImage.Origin = fullPath;
                    postedImage.Type = image.Type;


                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);

                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //public IActionResult Upload()
        //{
        //    try
        //    {
        //        var files = Request.Form.Files;
        //        var folderName = Path.Combine("StaticFiles", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (files.Any(f => f.Length == 0))
        //        {
        //            return BadRequest();
        //        }

        //        foreach (var file in files)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);

        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //        }

        //        return Ok("All the files are successfully uploaded.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}