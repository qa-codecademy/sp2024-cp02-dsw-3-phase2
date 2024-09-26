using ArtShop.DTO.ArtImageDTOs;
using ArtShop.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Mappers.ImageMapper
{
    public static class ImageMapper
    {
        public static ArtImage ToArtImage(this AddImageDto image)
        {
            return new()
            {
                Description = image.Description,
                Category = image.Category,
                Price = image.Price,
                Stock = image.Stock,
                ImageUrl = image.ImageUrl,
                CreatedAt = image.CreatedAt
            };

        }
    }
}
