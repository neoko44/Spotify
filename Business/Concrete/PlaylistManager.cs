using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PlaylistManager : IPlaylistService
    {
        private IPlaylistDal _playlistDal;
        private IUserPlaylistDal _userPlaylistDal;
        private ITokenHelper _tokenHelper;
        private IUserDal _userDal;
        private IUserLibraryDal _userLibraryDal;

        public PlaylistManager(IPlaylistDal playlistDal, ITokenHelper tokenHelper, IUserDal userDal, IUserPlaylistDal userPlaylistDal, IUserLibraryDal userLibraryDal)
        {
            _playlistDal = playlistDal;
            _tokenHelper = tokenHelper;
            _userDal = userDal;
            _userPlaylistDal = userPlaylistDal;
            _userLibraryDal = userLibraryDal;
        }

        public IDataResult<Playlist> DeletePlaylist(int playlistId)
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

            var getPlaylist = _playlistDal.Get(p => p.UserId == user.Id && p.Id == playlistId);
            if(getPlaylist == null)
            {
                return new ErrorDataResult<Playlist>(Messages.PlaylistNotFound);
            }

            var getUserLibrary = _userLibraryDal.Get(ul => ul.UserId == user.Id && ul.PlaylistId == getPlaylist.Id);

            var getUserPlaylists = _userPlaylistDal.GetList(up => up.PlaylistId == getPlaylist.Id);

            foreach (var item in getUserPlaylists)
            {
                item.Status = false;
                item.EditedDate = DateTime.Now;
                _userPlaylistDal.Update(item);
            }

            getPlaylist.Status = false;
            getPlaylist.EditedDate = DateTime.Now;
            _playlistDal.Update(getPlaylist);

            getUserLibrary.Status = false;
            getUserLibrary.EditedDate = DateTime.Now;
            _userLibraryDal.Update(getUserLibrary);

            return new SuccessDataResult<Playlist>(Messages.PlaylistDeleted);

        }
        public async Task<IResult> UpdateName(UpdatePlaylistNameDto updatePlaylistNameDto)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<UpdatePlaylistNameDto>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<Playlist>(Messages.UserNotFound);
            }

            var getUserLibrary = _userLibraryDal.Get(ul => ul.UserId == user.Id && 
            ul.Type == Types.PlaylistType && ul.Name == updatePlaylistNameDto.Name);

            if(getUserLibrary == null)
            {
                return new ErrorDataResult<UserLibrary>(Messages.PlaylistNotFound);
            }

            var getPlaylist = _playlistDal.Get(p => p.Id == updatePlaylistNameDto.PlaylistId && p.UserId == user.Id);
            if (getPlaylist == null)
            {
                return new ErrorDataResult<Playlist>(Messages.PlaylistNotFound);
            }

            if (getPlaylist.Name == updatePlaylistNameDto.Name)
            {
                return new ErrorDataResult<Playlist>(Messages.NameMustBeDifferent);
            }

            getPlaylist.Name = updatePlaylistNameDto.Name;
            getPlaylist.UserId = user.Id;
            getPlaylist.Status = true;
            getPlaylist.EditedDate = DateTime.Now;

            getUserLibrary.Status = true;
            getUserLibrary.Name = updatePlaylistNameDto.Name;
            getUserLibrary.EditedDate = DateTime.Now;

            _userLibraryDal.Update(getUserLibrary);
            _playlistDal.Update(getPlaylist);


            return new SuccessDataResult<Playlist>(Messages.SuccessfullyUpdated);
        }
    }
}
