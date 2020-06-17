using AutoMapper;
using Ecomak.Data.Entities;
using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data
{
    public class EcomakProfile : Profile
    {
        public EcomakProfile()
        {
            this.CreateMap<ProductEntity, Product>()
                .ReverseMap();
            this.CreateMap<TrEntity, Tr>()
             .ReverseMap();
            this.CreateMap<CategoryEntity, Category>()
                .ReverseMap();
            this.CreateMap<PromotionEntity, Promotion>()
                .ReverseMap();
            this.CreateMap<CommentaryEntity, Commentary>()
                .ReverseMap();
            this.CreateMap<SubscribeEntity, Subscribe>()
                .ReverseMap();
            this.CreateMap<QuoteEntity, Quote>()
                .ReverseMap();
            this.CreateMap<ImageEntity, Image>()
                .ReverseMap();
        }
    }
}
