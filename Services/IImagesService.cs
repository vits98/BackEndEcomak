using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Services
{
    public interface IImagesService
    {
        Task<Image> GetImageAsyncByIdImage(int id);
        Task<Image> CreateImage(Image image);
        Task<bool> DeleteImageAsync(int id);
        Task<Image> UpdateImageAsync(int id, Image newImage);
    }
}
