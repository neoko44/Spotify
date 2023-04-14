using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FollowManager : IFollowService
    {
        private IFollowDal _followDal;
        private ITokenHelper _tokenHelper;
        private IUserDal _userDal;
        private IPlaylistDal _playlistDal;

        public FollowManager(IFollowDal followDal, IUserDal userDal, ITokenHelper tokenHelper, IPlaylistDal playlistDal)
        {
            _followDal = followDal;
            _userDal = userDal;
            _tokenHelper = tokenHelper;
            _playlistDal = playlistDal;
        }

        public IResult FollowPlaylist(int playlistId)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var checkFollow = _followDal.Get(f=>f.UserId == user.Id && f.PlaylistId == playlistId && f.Status ==true);
            if (checkFollow != null)
            {
                return new ErrorDataResult<Follow>(Messages.PlaylistAlreadyFollowing);
            }

            var checkUnfollow = _followDal.Get(f => f.UserId == user.Id && f.PlaylistId == playlistId && f.Status == false);
            if (checkUnfollow != null)
            {
                checkUnfollow.Status = true;
                checkUnfollow.EditedDate = DateTime.Now;
                _followDal.Update(checkUnfollow);
            }

            var getPlaylist = _playlistDal.Get(p=>p.Id == playlistId);
            if (getPlaylist == null)
            {
                return new ErrorDataResult<Playlist>(Messages.PlaylistNotFound);
            }
            
            var getPlaylistsOfCurrentUser = _playlistDal.GetList(p => p.UserId == user.Id).ToList();
            foreach (var eachPlaylist in getPlaylistsOfCurrentUser)
            {
                if (eachPlaylist.Id == playlistId)
                    return new ErrorDataResult<Playlist>(Messages.CantFollowYourself);
            }

            Follow follow = new()
            {
                PlaylistId = playlistId,
                Status = true,
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _followDal.Add(follow);

            return new SuccessDataResult<Follow>(Messages.PlaylistFollowed);   
            
        }

        public IResult UnfollowPlaylist(int playlistId)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            if (user == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var getPlaylist = _playlistDal.Get(p => p.Id == playlistId);
            if (getPlaylist == null)
            {
                return new ErrorDataResult<Playlist>(Messages.PlaylistNotFound);
            }

            var checkFollow = _followDal.Get(f => f.UserId == user.Id && f.PlaylistId == playlistId && f.Status == true);
            if (checkFollow != null)
            {
                checkFollow.Status = false;
                checkFollow.EditedDate = DateTime.Now;
                _followDal.Update(checkFollow);
            }

            return new SuccessDataResult<Follow>(Messages.PlaylistUnfollowed);

        }

        public IResult FollowUser(int userId)
        {

        }

    }
}
