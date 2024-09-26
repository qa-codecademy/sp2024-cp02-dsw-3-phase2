using ArtShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageContoller : ControllerBase
    {
        private readonly IArtImageService _artImageService;

    }
}
