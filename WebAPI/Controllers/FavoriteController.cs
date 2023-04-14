using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        IUserFavoriteService _userFavoriteService;

        public FavoriteController(IUserFavoriteService userFavoriteService)
        {
            _userFavoriteService = userFavoriteService;
        }

        [HttpPost("add/favorites")]
        public IActionResult AddToFavorites(string trackId)
        {
            var result = _userFavoriteService.Add(trackId);
            return Ok(result.Result);
            
        }

        [HttpPost("delete/favorites/track")]
        public IActionResult RemoveTrackFromFavorites(string trackId)
        {
            var result = _userFavoriteService.Delete(trackId);
            return Ok(result);
        }
    }
}
