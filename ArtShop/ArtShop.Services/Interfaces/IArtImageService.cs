using ArtShop.DTO.ArtImageDTOs;
using ArtShop.Entities.Entities;
using ArtShop.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Interfaces
{
    public interface IArtImageService
    {
        public PaginatedResult<ArtImage> GetArtImages(int pageNumber, int pageSize, Category? category, bool? inStock);
        public ArtImage GetImageById(int id);
        public List<User> GetUsers();
        public string AddImage(string filePath);
        public string DeleteImage(string id);
    }
}
