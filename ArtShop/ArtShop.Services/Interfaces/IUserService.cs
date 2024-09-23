using ArtShop.DTO.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Interfaces
{
    public interface IUserService
    {
        public string Register(RegisterUserDto registerUser);
        public string Login(LoginUserDto loginUser);
    }
}
