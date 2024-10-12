using ArtShop.DTO.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShop.Services.Interfaces
{
    public interface IUserService
    {
        public RegisterUserResultDto Register(RegisterUserDto registerUser);
        public LoginUserResultDto Login(LoginUserDto loginUser);
        public UpdateUserResultDto Update(Guid userId,UpdateUserDto updateUser);
        public UserInfoDto UserInfo(Guid id);
    }
}
