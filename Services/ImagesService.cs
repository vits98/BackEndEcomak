using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecomak.Data.Entities;
using Ecomak.Data.Repository;
using Ecomak.Exceptions;
using Ecomak.Models;

namespace Ecomak.Services
{
    public class ImagesService : IImagesService
    {
        private IEcomakRepository EcomakRepository;
        private readonly IMapper mapper;

        public ImagesService(IEcomakRepository _EcomakRepository, IMapper _mapper)
        {
            this.EcomakRepository = _EcomakRepository;
            this.mapper = _mapper;
        }

        private async Task validateImage(int id)
        {
            var quote = await EcomakRepository.GetImageAsyncByIdImage(id);
            if (quote == null)
            {
                throw new NotFoundItemException($"cannot found product with id:{id}");
            }
        }

        public async Task<Image> CreateImage(Image image)
        {

            var imageEntity = mapper.Map<ImageEntity>(image);
            EcomakRepository.CreateImage(imageEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Image>(imageEntity);
            }

            throw new Exception("there where and error with the DB");
        }

      
        public async Task<Image> GetImageAsyncByIdImage(int id)
        {
            var ImageEntity = await EcomakRepository.GetImageAsyncByIdImage(id);
            if (ImageEntity == null)
            {
                throw new NotFoundItemException("Product not found");
            }
            return mapper.Map<Image>(ImageEntity);
        }

        public async Task<Image> UpdateImageAsync(int id, Image Image)
        {
            if (id != Image.Id)
            {
                throw new InvalidOperationException("URL id needs to be the same as Image id");
            }
            await validateImage(id);
            Image.Id = id;
            var ImageEntity = mapper.Map<ImageEntity>(Image);
            EcomakRepository.UpdateImage(ImageEntity);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return mapper.Map<Image>(ImageEntity);
            }

            throw new Exception("There were an error with the DB");
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            await validateImage(id);
            await EcomakRepository.DeleteImageAsync(id);
            if (await EcomakRepository.SaveChangesAsync())
            {
                return true;
            }
            return false;
        }


    }
}
