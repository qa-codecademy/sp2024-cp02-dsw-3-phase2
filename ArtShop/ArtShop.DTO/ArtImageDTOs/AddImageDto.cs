using ArtShop.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.DTO.ArtImageDTOs
{
    public class AddImageDto
    {
        public string Description { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public bool Stock { get; set; } = true;
        public string ImageUrl { get; set; }
        public string CreatedAt { get; set; }
    }
}
