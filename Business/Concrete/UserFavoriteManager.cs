using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserFavoriteManager : IUserFavoriteService
    {
        IUserFavoriteDal _userFavoriteDal;
        ITokenHelper _tokenHelper;
        IUserDal _userDal;
        IFavoriteDal _favoriteDal;
        ILibraryDal _libraryDal;
        IUserLibraryDal _userLibraryDal;
        ITrackPoolService _trackPoolService;

        public UserFavoriteManager(IUserFavoriteDal userFavoriteDal, ITokenHelper tokenHelper, IUserDal userDal, IFavoriteDal favoriteDal, ILibraryDal libraryDal, IUserLibraryDal userLibraryDal, ITrackPoolService trackPoolService)
        {
            _userFavoriteDal = userFavoriteDal;
            _tokenHelper = tokenHelper;
            _userDal = userDal;
            _favoriteDal = favoriteDal;
            _libraryDal = libraryDal;
            _userLibraryDal = userLibraryDal;
            _trackPoolService = trackPoolService;
        }

        public async Task<IResult> Add(string trackId)
        {
            var tokenClaim = _tokenHelper.GetTokenInfo();
            if (tokenClaim.Data == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }


            var user = _userDal.Get(u => u.Email == tokenClaim.Data.Email);
            var tracks = await _trackPoolService.GetTrackPoolAsync();
            var favorite = _favoriteDal.Get(f => f.UserId == user.Id);
            var track = tracks.Data.Find(t => t.Id == trackId);
            var userFavorites = _userFavoriteDal.GetList(uf => uf.UserId == user.Id && uf.Status == true);
            var library = _libraryDal.Get(ul => ul.UserId == user.Id);

            if (userFavorites.Count != 0)
            {
                foreach (var liked in userFavorites)
                {
                    if (liked.TrackId.Contains(trackId))
                    {
                        return new ErrorDataResult<UserFavorite>(Messages.TrackAlreadyExists);
                    }
                }
            }

            if (track == null)
            {
                return new ErrorDataResult<TrackDto>(Messages.TrackNotFound);
            }
            if (user == null)
            {
                return new ErrorDataResult<UserFavorite>(Messages.UserNotFound);
            }
            if (library == null)
            {
                return new ErrorDataResult<UserFavorite>(Messages.LibraryNotFound);
            }


            UserFavorite userFavorite = new()
            {
                FavoriteId = favorite.Id,
                UserId = user.Id,
                TrackId = track.Id,
                Status = true,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };

            _userFavoriteDal.Add(userFavorite);


            var getUserLibrary = _userLibraryDal.Get(ul => ul.Name == Types.LikedTracksName);
            if (getUserLibrary != null)
            {
                getUserLibrary.TrackCount += 1;
                getUserLibrary.EditedDate = DateTime.Now;
                _userLibraryDal.Update(getUserLibrary);
            }
            else if (getUserLibrary == null)
            {
                UserLibrary userLibrary = new()
                {
                    LibraryId = library.Id,
                    Name = Types.LikedTracksName,
                    TrackCount = userFavorites.Count + 1,
                    Type = Types.PlaylistType,
                    UserId = user.Id,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    EditedDate= DateTime.Now,
                };

                _userLibraryDal.Add(userLibrary);
            }



            favorite.Status = true;
            favorite.UserId = user.Id;
            favorite.EditedDate = DateTime.Now;

            _favoriteDal.Update(favorite);
            return new SuccessDataResult<UserFavorite>(Messages.TrackAddedToFavorites);
        }

        public IResult Delete(string trackId)
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

            var track = _userFavoriteDal.Get(uf=>uf.TrackId == trackId);
            if (track == null)
            {
                return new ErrorDataResult<TrackDto>(Messages.TrackNotFound);
            }

            track.Status = false;
            track.EditedDate = DateTime.Now;
            _userFavoriteDal.Update(track);

            return new SuccessDataResult<Favorite>(Messages.TrackRemovedFromFavorites);

        }
    }
}
