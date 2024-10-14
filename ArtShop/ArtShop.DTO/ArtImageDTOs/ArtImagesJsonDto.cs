using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.DTO.ArtImageDTOs
{
    public class ArtImagesJsonDto
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public bool Stock { get; set; }
        public string ImageUrl { get; set; }
    }
}
