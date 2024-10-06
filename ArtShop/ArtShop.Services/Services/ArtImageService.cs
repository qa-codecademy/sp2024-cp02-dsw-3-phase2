using ArtShop.DataAcces;
using ArtShop.DTO.ArtImageDTOs;
using ArtShop.DTO.UserDTOs;
using ArtShop.Entities.Entities;
using ArtShop.Entities.Enums;
using ArtShop.Mappers.ImageMapper;
using ArtShop.Mappers.UserMapper;
using ArtShop.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;

namespace ArtShop.Services.Services
{
    public class ArtImageService : IArtImageService
    {
        private readonly ArtShopDbContext _dbContext;

        public ArtImageService(ArtShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AddImageResultDto AddImage(AddImageDto addimage,Guid userId)
        {
            if (string.IsNullOrEmpty(addimage.Description))
            {
                return new AddImageResultDto { Success = false, Message = "Image Description Cannot be epty!" };
            }

            if (string.IsNullOrEmpty(addimage.ImageUrl))
            {
                return new AddImageResultDto { Success = false, Message = "Must provide ImageUrl!" };
            }

            
            var user = _dbContext.Users.Find(userId);

            if(user == null)
            {
                return new AddImageResultDto { Success = false,Message = "User Does not Exist" };
            }

            var artImage = addimage.ToArtImage();

            ArtImage image = new ArtImage()
            {
                Id = Guid.NewGuid(),
                Description = artImage.Description,
                ImageUrl = artImage.ImageUrl,
                Category = artImage.Category,
                Price = artImage.Price,
                Stock = true,
                UserId = userId,
                CreatedAt = DateTime.Now.ToString(),
            };

            //uste serializacija vo JSON i zapisuvanje vo Images.json

            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return new AddImageResultDto { Success = true, Message = "Image Added Succesfuly" };
        }

        public string DeleteImage(Guid id)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<ArtImageDto> GetArtImages(int pageNumber, Category? category, bool? inStock)
        {
            var query = _dbContext.Images.AsQueryable();

            if (category.HasValue)
            {
                query = query.Where(a => a.Category == category.Value);
            }

            if (inStock.HasValue)
            {
                query = query.Where(a => a.Stock == inStock.Value);
            }

            var totalCount = query.Count();
 
            var artImagesDto = query.Skip((pageNumber - 1) * 12)
                                    .Take(12)
                                    .Select(a => new ArtImageDto
                                    {
                                        Description = a.Description,
                                        ImageUrl = a.ImageUrl,
                                        Price = a.Price,
                                        Stock = a.Stock,
                                        CreatedAt = a.CreatedAt,
                                        Category = a.Category.ToString(),
                                        UserId = a.UserId
                                    })
                                    .ToList();

            return new PaginatedResult<ArtImageDto>
            {
                Data = artImagesDto,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = 12,
                TotalPages = (int)Math.Ceiling(totalCount / (double)12)
            };
        }


        public ArtImage GetImageById(Guid id)
        {
            var image = _dbContext.Images.Find(id);
            if(image == null)
            {
                throw new Exception("There is no such image");
            }

            return image;
        }

        public List<UserDto> GetUsers()
        {
            var usersWithImagesDto = _dbContext.Users
                                                .Where(x => x.Images != null)
                                                .Select(user => user.ToUserDto())
                                                .ToList();

            return new List<UserDto>
            {
                usersWithImagesDto
            };
        }
    }
}
