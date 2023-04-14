using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Concrete.JsonEntity;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserPlaylistManager : IUserPlaylistService
    {
        private IUserPlaylistDal _userPlaylistDal;
        private IPlaylistDal _playlistDal;
        private IUserDal _userDal;
        private ITokenHelper _tokenHelper;
        private ITrackPoolService _trackPoolService;
        private IUserLibraryDal _userLibraryDal;
        private ILibraryDal _libraryDal;

        public UserPlaylistManager(IUserPlaylistDal userPlaylistDal, IUserDal userDal, ITokenHelper tokenHelper, IPlaylistDal playlistDal, ITrackPoolService trackPoolService, IUserLibraryDal userLibraryDal, ILibraryDal libraryDal)
        {
            this._userPlaylistDal = userPlaylistDal;
            _userDal = userDal;
            _tokenHelper = tokenHelper;
            _playlistDal = playlistDal;
            _trackPoolService = trackPoolService;
            _userLibraryDal = userLibraryDal;
            _libraryDal = libraryDal;
        }

        public async Task<IResult> AddAlbumToPlaylist(string albumId)
        {

            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<Playlist>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<Playlist>(Messages.UserNotFound);
            }

            var library = _libraryDal.Get(ul => ul.UserId == user.Id);
            if (library == null)
            {
                return new ErrorDataResult<Library>(Messages.LibraryNotFound);
            }

            var getAlbum = await _trackPoolService.GetAlbumAsync(albumId);
            if (!getAlbum.Success)
            {
                return new ErrorDataResult<Album>(Messages.AlbumNotFound);
            }

            var getPlaylist = _playlistDal.Get(p => p.Name == getAlbum.Data.name && p.UserId == user.Id);
            if (getPlaylist != null)
            {
                return new ErrorDataResult<Playlist>(Messages.PlaylistAlreadyExists);
            }

            Playlist playlist = new()
            {
                Name = getAlbum.Data.name,
                Status = true,
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
                
            };
            _playlistDal.Add(playlist);


            List<ArtistDto> artists = new();
            foreach (var artist in getAlbum.Data.artists)
            {
                ArtistDto artistDto = new()
                {
                    Name = artist.name
                };
                artists.Add(artistDto);
            }

            foreach (var eachTrack in getAlbum.Data.tracks.items)
            {
                UserPlaylist userPlaylist = new()
                {
                    PlaylistId = playlist.Id,
                    TrackId = eachTrack.id,
                    UserId = user.Id,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    EditedDate = DateTime.Now,
                };
                _userPlaylistDal.Add(userPlaylist);
            }

            var getUserLibrary = _userLibraryDal.Get(ul => ul.Name == getAlbum.Data.name);

            if (getUserLibrary == null)
            {
                var getTrackCount = _userPlaylistDal.GetList(up => up.PlaylistId == playlist.Id).Count();
                UserLibrary userLibrary = new()
                {
                    LibraryId = library.Id,
                    Name = getAlbum.Data.name,
                    TrackCount = getTrackCount,
                    Type = Types.AlbumType,
                    UserId = user.Id,
                    Status = true,
                    PlaylistId = playlist.Id,
                    CreatedDate = DateTime.Now,
                    EditedDate = DateTime.Now,
                };

                _userLibraryDal.Add(userLibrary);
            }

            return new SuccessDataResult<Album>(Messages.AlbumAdded);
        }
        public IDataResult<Playlist> AddTrackToPlaylist(AddToPlaylistDto addToPlaylistDto)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<Playlist>(Messages.UserNotFound);
            }

            var tracks = _trackPoolService.GetTrackPoolAsync().Result.Data;
            var track = tracks.Find(t => t.Id == addToPlaylistDto.TrackId);
            if (track == null)
            {
                return new ErrorDataResult<Playlist>(Messages.TrackNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<Playlist>(Messages.UserNotFound);
            }

            var getLibrary = _libraryDal.Get(l => l.UserId == user.Id);

            var getPlaylist = _playlistDal.Get(p => p.Name == addToPlaylistDto.Name && p.UserId == user.Id && p.Id == addToPlaylistDto.PlaylistId);
            if (getPlaylist == null)
            {
                return new ErrorDataResult<Playlist>(getPlaylist, Messages.PlaylistNotFound);
            }

            var getUserPlaylist = _userPlaylistDal.GetList(up => up.TrackId == addToPlaylistDto.TrackId && up.UserId == user.Id && up.PlaylistId == getPlaylist.Id).ToList();

            foreach (var item in getUserPlaylist)
            {
                if (item.TrackId == track.Id)
                    return new ErrorDataResult<Playlist>(Messages.TrackAlreadyExists);
            }

            UserPlaylist userPlaylist = new()
            {
                PlaylistId = getPlaylist.Id,
                UserId = user.Id,
                TrackId = track.Id,
                Status = true,
                CreatedDate = DateTime.UtcNow,
                EditedDate = DateTime.UtcNow,
            };
            _userPlaylistDal.Add(userPlaylist);

            var getUserLibrary = _userLibraryDal.Get(ul => ul.Name == addToPlaylistDto.Name
            && ul.LibraryId == getLibrary.Id && ul.UserId == user.Id&&ul.PlaylistId == userPlaylist.PlaylistId);

            getUserLibrary.TrackCount += 1;
            getUserLibrary.EditedDate = DateTime.Now;

            _userLibraryDal.Update(getUserLibrary);

            return new SuccessDataResult<Playlist>(Messages.AddedToPlaylist);
        }
        public IDataResult<string> CreatePlaylist(string name)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<string>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<string>(Messages.UserNotFound);
            }

            Playlist playlist = new()
            {
                Status = true,
                UserId = user.Id,
                Name = name,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _playlistDal.Add(playlist);

            var getLibrary = _libraryDal.Get(l => l.UserId == user.Id);
            UserLibrary userLibrary = new()
            {
                LibraryId = getLibrary.Id,
                Type = Types.PlaylistType,
                Name = playlist.Name,
                UserId = user.Id,
                Status = true,
                TrackCount = 0,
                PlaylistId = playlist.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _userLibraryDal.Add(userLibrary);

            return new SuccessDataResult<string>($" {playlist.Id} ", Messages.PlaylistCreated);
        }

        
    }
}
