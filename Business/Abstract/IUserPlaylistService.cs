using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserPlaylistService
    {
        IDataResult<string> CreatePlaylist(string name);
        IDataResult<Playlist> AddTrackToPlaylist(AddToPlaylistDto addToPlaylistDto);
        Task<IResult> AddAlbumToPlaylist(string albumId);
    }
}
