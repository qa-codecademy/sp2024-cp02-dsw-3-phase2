using ArtShop.DTO.ArtImageDTOs;
using ArtShop.DTO.CartDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Interfaces
{
    public interface ICartService
    {
        public CheckOutResult CheckOutAuthorized(List<Guid> imageId, Guid userId);
        public CheckOutResult CheckOutUnauthorized(List<Guid> imageId);
        public List<ArtImageDto> BoughtImages (Guid userId);
    }
}
