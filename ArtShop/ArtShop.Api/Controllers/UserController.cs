using ArtShop.DataAcces;
using ArtShop.DTO.UserDTOs;
using ArtShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.Security.Claims;

namespace ArtShop.Api.Controllers
{
    [Authorize]
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
        public IActionResult Register([FromBody] RegisterUserDto registerUser)
        {
            var result = _userService.Register(registerUser);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("login")]
        [AllowAnonymous]
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
        public IActionResult Update(string userName, [FromBody] UpdateUserDto updateUser)
        {
            var result = _userService.Update(userName, updateUser);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
        [HttpGet("userInfo")]
        public IActionResult UserInfo()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null)
            {
                return Unauthorized("User is not logged in.");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var userInfo = _userService.UserInfo(userId);

            if(userInfo == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(new { userInfo });
        }
    }
}
