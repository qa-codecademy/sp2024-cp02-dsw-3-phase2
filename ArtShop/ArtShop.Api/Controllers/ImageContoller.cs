using ArtShop.DTO.ArtImageDTOs;
using ArtShop.Entities.Enums;
using ArtShop.Services.Interfaces;
using ArtShop.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using XAct.Messages;

namespace ArtShop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageContoller : ControllerBase
    {
        private readonly IArtImageService _artImageService;
        public ImageContoller(IArtImageService artImageService)
        {
            _artImageService = artImageService;
        }

        [HttpPost("addImage")]
        public IActionResult AddImage([FromBody] AddImageDto addImageDto)
        {
            try
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

                if (addImageDto == null)
                {
                    return BadRequest("Invalid image data.");
                }

                var result = _artImageService.AddImage(addImageDto, userId);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                return Ok(new { result.Message });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("delete{id}")]
        public IActionResult DeleteImage(string id)
        {
            try
            {
                if(!Guid.TryParse(id,out Guid imageId))
                {
                    throw new Exception("Id canot be parsed");
                }
                var imageDelete = _artImageService.DeleteImage(imageId);

                return Ok(new {imageDelete.Message});
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetImages")]
        public IActionResult GetArtImages([FromQuery] int pageNumber = 1, [FromQuery] Category? category = null, [FromQuery] bool? inStock = null)
        {
            try
            {
                var paginatedImages = _artImageService.GetArtImages(pageNumber, category, inStock);
                return Ok(new{ paginatedImages});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("GetById{id}")]
        public IActionResult GetImageById(Guid id)
        {
            try
            {
                var image = _artImageService.GetImageById(id);
                if(image == null)
                {
                    return NotFound(new { Message = "There is no such image" });
                }
                return Ok(new { image });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _artImageService.GetUsers();
                return Ok(new { users });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
