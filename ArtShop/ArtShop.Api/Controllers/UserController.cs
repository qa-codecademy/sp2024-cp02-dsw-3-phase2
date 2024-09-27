using ArtShop.DataAcces;
using ArtShop.DTO.UserDTOs;
using ArtShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace ArtShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        //public IActionResult Register(RegisterUserDto user)
        //{
        //    return Ok(_userService.Register(user));
        //}

        public IActionResult Register([FromBody] RegisterUserDto registerUser)
        {
            var result = _userService.Register(registerUser); // Call the service

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("login")]
        [AllowAnonymous]

        //public IActionResult Login(LoginUserDto user)
        //{
        //    var token = _userService.Login(user);
        //    return Ok(token);
        //}

        public IActionResult Login([FromBody] LoginUserDto loginUser)
        {
            var result = _userService.Login(loginUser);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                token = result.Token,
                userFullName = result.UserFullName
            });
        }

        [HttpPut("{userName}")]
        [AllowAnonymous]
        public IActionResult Update(string userName, [FromBody] UpdateUserDto updateUser)
        {
            var result = _userService.Update(userName, updateUser);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }



        //public IActionResult Update(string userName,UpdateUserDto userDto)
        //{
        //        return Ok(_userService.Update(userName,userDto));
        //}
    }
}
