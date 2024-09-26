using ArtShop.DataAcces;
using ArtShop.DTO.ArtImageDTOs;
using ArtShop.Entities.Entities;
using ArtShop.Entities.Enums;
using ArtShop.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Services
{
    public class ArtImageService : IArtImageService
    {
        private readonly ArtShopDbContext _dbContext;

        public string AddImage(string filePath)
        {
            throw new NotImplementedException();
            //var jsonData = File.ReadAllText(filePath);
            //var modelList = JsonConvert.DeserializeObject<List<ArtImage>>(jsonData);

            //var imagesDb = _dbContext.Images;
            //imagesDb.AddRange(modelList);
            //_dbContext.SaveChanges();

            //return "Images Added Succesfuly";
        }

        public string DeleteImage(string id)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<ArtImage> GetArtImages(int pageNumber, int pageSize, Category? category, bool? inStock)
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

            var artImages = query.Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            var totalCount = query.Count();

            return new PaginatedResult<ArtImage>
            {
                Data = artImages,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public ArtImage GetImageById(int id)
        {
            var image = _dbContext.Images.Find(id);
            if(image == null)
            {
                throw new Exception("There is no such image");
            }

            return image;
        }

        public List<User> GetUsers()
        {
            var usersWithImages = _dbContext.Users.Where(x => x.Images != null).ToList();

            return usersWithImages;
        }
    }
}
