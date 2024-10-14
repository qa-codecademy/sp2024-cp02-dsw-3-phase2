using ArtShop.DataAcces;
using ArtShop.DTO.ArtImageDTOs;
using ArtShop.DTO.CartDto;
using ArtShop.Mappers.ImageMapper;
using ArtShop.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ArtShopDbContext _dbContext;
        public CartService(ArtShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CheckOutResult CheckOutAuthorized(List<Guid> imageId, Guid userId)
        {

            var images = _dbContext.Images.Where(i => imageId.Contains(i.Id)).ToList();

            if (images == null || images.Count == 0)
            {
                return new CheckOutResult
                {
                    IsChecked = false,
                    Message = "There are no images"
                };
            }

            foreach (var image in images)
            {
                image.BoughtByUserId = userId;
            }
           
            foreach (var image in images)
            {
                image.Stock = false; 

                image.SoldByUserId = image.UserId;
            }

             _dbContext.SaveChanges();

            return new CheckOutResult
            {
                IsChecked = true,
                Message = "Images Bought Succesfuly"
            };
        }

        public CheckOutResult CheckOutUnauthorized(List<Guid> imageId)
        {
            var images = _dbContext.Images.Where(i => imageId.Contains(i.Id)).ToList();

            if (images == null || images.Count == 0)
            {
                return new CheckOutResult
                {
                    IsChecked = false,
                    Message = "There are no images"
                };
            }

            foreach (var image in images)
            {
                image.Stock = false;

                image.SoldByUserId = image.UserId;
            }

            _dbContext.SaveChanges();

            return new CheckOutResult
            {
                IsChecked = true,
                Message = "Images Bought Succesfuly"
            };
        }

        public List<ArtImageDto> BoughtImages(Guid userId)
        {
            if(userId == null)
            {
                throw new Exception("User does not exist");
            }

            var boughtImages = _dbContext.Images
                                         .Where(i => i.BoughtByUserId == userId)
                                         .Select(i => i.ToBoughtImages())
                                         .ToList();

            return boughtImages;
        }
    }
}
 