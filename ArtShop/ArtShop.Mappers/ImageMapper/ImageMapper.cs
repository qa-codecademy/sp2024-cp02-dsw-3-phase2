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
            var price = int.Parse(image.Price);

            return new()
            {
                Description = image.Description,
                Category = image.Category,
                Price = price,
                Stock = image.Stock,
                ImageUrl = image.ImageUrl
            };

        }
        public static AddImageDto ToArtImageDto(this ArtImage image)
        {
            return new()
            {
                Description = image.Description,
                Category = image.Category,
                Price = image.Price.ToString(),
                Stock = image.Stock,
                ImageUrl = image.ImageUrl,
                //UserId = image.UserId
            };
        }
    }
}
