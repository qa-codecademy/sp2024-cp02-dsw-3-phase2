using ArtShop.DTO.UserDTOs;
using ArtShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Register(RegisterUserDto user)
        {
            return Ok(_userService.Register(user));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginUserDto user)
        {
            var token = _userService.Login(user);
            return Ok(token);
        }
    }
}
