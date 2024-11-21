using ArtShop.DTO.ArtImageDTOs;
using ArtShop.DTO.UserDTOs;
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
        public PaginatedResult<ArtImageDto> GetArtImages(string username, string searchTerm,int pageNumber, Category? category, bool? inStock, bool sortByPriceAsc);
        public ArtImage GetImageById(Guid id);
        public List<UserDto> GetUsers();
        public AddImageResultDto AddImage(AddImageDto addimage,Guid userId);
        public DeleteImageResponse DeleteImage(Guid id);
        public Task ImportImagesFromJson(Guid userId);
    }
}
