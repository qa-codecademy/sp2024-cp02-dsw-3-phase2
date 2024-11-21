using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.DTO.UserDTOs
{
    public class RegisterUserResponseDto
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
