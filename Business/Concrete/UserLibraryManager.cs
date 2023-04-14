using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Dtos;

namespace Business.Concrete
{
    public class UserLibraryManager : IUserLibraryService
    {
        IUserFavoriteDal _userFavoriteDal;
        IFavoriteDal _favoriteDal;
        ITokenHelper _tokenHelper;
        IUserDal _userDal;
        IPlaylistDal _playlistDal;
        IUserPlaylistDal _userPlaylistDal;
        IUserLibraryDal _userLibraryDal;

        public UserLibraryManager(IUserFavoriteDal userFavoriteDal, ITokenHelper tokenHelper, IUserDal userDal, IFavoriteDal favoriteDal, IPlaylistDal playlistDal, IUserPlaylistDal userPlaylistDal, IUserLibraryDal userLibraryDal)
        {
            _userFavoriteDal = userFavoriteDal;
            _tokenHelper = tokenHelper;
            _userDal = userDal;
            _favoriteDal = favoriteDal;
            _playlistDal = playlistDal;
            _userPlaylistDal = userPlaylistDal;
            _userLibraryDal = userLibraryDal;
        }

        public IDataResult<List<LibraryDto>> GetUserLibrary()
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<List<LibraryDto>>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<List<LibraryDto>>(Messages.UserNotFound);
            }

            var userFavorites = _userFavoriteDal.GetList(uf => uf.UserId == user.Id && uf.Status == true).ToList();

            List<LibraryDto> list = new();

            LibraryDto libraryDto = new()
            {
                Name = Types.LikedTracksName,
                TrackCount = userFavorites.Count,
                Type = Types.PlaylistType
            };

            if (userFavorites.Count > 0) { list.Add(libraryDto); }


            var userPlaylists = _userPlaylistDal.GetList(up => up.UserId == user.Id && up.Status == true).ToList();

            if (userPlaylists.Count > 0)
            {
                var Playlists = _playlistDal.GetList(p => p.UserId == user.Id && p.Status == true).ToList();

                foreach (var playlist in Playlists)
                {
                    var userPlaylistCount = userPlaylists.Count(up => up.PlaylistId == playlist.Id);
                    var userLibrary = _userLibraryDal.Get(ul => ul.Status == true && ul.PlaylistId == playlist.Id);

                    LibraryDto libraryDtoPlaylist = new()
                    {
                        Name = playlist.Name,
                        TrackCount = userPlaylistCount,
                        Type = userLibrary.Type,
                    };
                    list.Add(libraryDtoPlaylist);
                }
            }

            return new SuccessDataResult<List<LibraryDto>>(list);
        }
    }
}
