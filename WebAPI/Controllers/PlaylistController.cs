using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        IUserPlaylistService _userPlaylistService;
        IPlaylistService _playlistService;
        IFollowService _followService;

        public PlaylistController(IUserPlaylistService userPlaylistService, IPlaylistService playlistService, IFollowService followService)
        {
            _userPlaylistService = userPlaylistService;
            _playlistService = playlistService;
            _followService = followService;
        }


        [HttpPost("add")]
        public IActionResult CreatePlaylist(string name)
        {
            var result = _userPlaylistService.CreatePlaylist(name);
            return Ok(result);
        }

        [HttpPost("delete")]
        public IActionResult DeletePlaylist(int playlistId)
        {
            var result = _playlistService.DeletePlaylist(playlistId);
            return Ok(result);
        }

        [HttpPost("update/name")]
        public IActionResult UpdatePlaylistName(UpdatePlaylistNameDto updatePlaylistNameDto)
        {
            var result = _playlistService.UpdateName(updatePlaylistNameDto);
            return Ok(result);
        }

        [HttpPost("add/track")]
        public IActionResult AddTrackToPlaylist(AddToPlaylistDto addToPlaylistDto)
        {
            var result = _userPlaylistService.AddTrackToPlaylist(addToPlaylistDto);
            return Ok(result);
        }

        [HttpPost("add/album")]
        public async Task<IActionResult> AddAlbumToPlaylist(string albumId)
        {
            var result = await _userPlaylistService.AddAlbumToPlaylist(albumId);
            return Ok(result.Message);
        }

        [HttpPost("follow/playlist")]
        public IActionResult FollowPlaylist(int playlistId)
        {
            var result = _followService.FollowPlaylist(playlistId);
            return Ok(result);
        }

        [HttpPost("unfollow/playlist")]
        public IActionResult UnfollowPlaylist(int playlistId)
        {
            var result = _followService.UnfollowPlaylist(playlistId);
            return Ok(result);
        }


    }
}
