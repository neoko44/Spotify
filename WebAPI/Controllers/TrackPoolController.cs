using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackPoolController : ControllerBase
    {
        ITrackPoolService _trackPoolService;

        public TrackPoolController(ITrackPoolService trackPoolService)
        {
            _trackPoolService = trackPoolService;
        }

        [HttpGet("get/trackpool")]
        public IActionResult GetPlaylist()
        {
            var result = _trackPoolService.GetTrackPoolAsync();
            return Ok(result.Result);
        }

    }
}
