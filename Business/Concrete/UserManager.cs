using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        ITokenHelper _tokenHelper;
        IUserOperationDal _operationDal;
        ILibraryDal _libraryDal;
        IFavoriteDal _favoriteDal;
        IUserLibraryDal _userLibraryDal;

        public UserManager(IUserDal userDal, ITokenHelper tokenHelper, IUserOperationDal operationDal, ILibraryDal libraryDal, IFavoriteDal favoriteDal, IUserLibraryDal userLibraryDal)
        {
            _userDal = userDal;
            _tokenHelper = tokenHelper;
            _operationDal = operationDal;
            _libraryDal = libraryDal;
            _favoriteDal = favoriteDal;
            _userLibraryDal = userLibraryDal;
        }


        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userDal.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<AccessToken> Register(RegisterDto registerDto)
        {
            if (registerDto.UserName == null || registerDto.UserName == "")
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNameNull);
            }
            if (registerDto.Password == null || registerDto.Password == "")
            {
                return new ErrorDataResult<AccessToken>(Messages.PasswordNull);
            }
            if (registerDto.Email == null || registerDto.Email == "")
            {
                return new ErrorDataResult<AccessToken>(Messages.EmailNull);
            }
            if (registerDto.FirstName == null || registerDto.FirstName == "")
            {
                return new ErrorDataResult<AccessToken>(Messages.FirstNameNull);
            }
            if (registerDto.LastName == null || registerDto.LastName == "")
            {
                return new ErrorDataResult<AccessToken>(Messages.LastNameNull);
            }
            if (!CheckEmail(registerDto.Email).Success)
            {
                return new ErrorDataResult<AccessToken>(Messages.MailExists);
            }
            if (!CheckUserName(registerDto.UserName).Success)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNameExists);
            }


            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            User user = new()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                RoleId = 1,
                Status = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _userDal.Add(user);

            UserOperation userOperation = new()
            {
                OperationId = 1,
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _operationDal.Add(userOperation);

            Library library = new()
            {
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _libraryDal.Add(library);

            Favorite favorite = new()
            {
                Status = false,
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
            };
            _favoriteDal.Add(favorite);

            var accessToken = CreateAccessToken(user);
            if (accessToken == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.AccessTokenError);
            }

            return new SuccessDataResult<AccessToken>(accessToken.Data, Messages.UserRegistered.ToString());
        }

        public IDataResult<User> LoginMail(string email, string password)
        {
            var userToCheck = GetByMail(email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (password == null)
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            if (!HashingHelper.VerifyPasswordHash(password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IDataResult<User> LoginUserName(string userName, string password)
        {

            var userToCheck = GetByUserName(userName);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (password == null)
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            if (!HashingHelper.VerifyPasswordHash(password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IDataResult<User> ChangePassword(string newPassword)
        //giriş yapan kullanıcının şifresini değiştir
        {
            var tokenClaim = _tokenHelper.GetTokenInfo(); // kullanıcının tokeni decrypt ediliyor ve
                                                          // buradan sonra gerçek kişi olup olmadığı doğrulanacak

            var user = _userDal.Get(x => x.Email == tokenClaim.Data.Email); //tokeni getir. token içerisindeki maile
                                                                            //karşılık gelen kullanıcıyı veritabanından getir


            if (HashingHelper.VerifyPasswordHash(newPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.NewPassMustDifferent);
            }
            else
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);//eski şifreyi kontrol et
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;
                user.EditedDate = DateTime.Now;

                _userDal.Update(user);
            }
            return new SuccessDataResult<User>(Messages.PassChangeSuccess);
        }



        public List<Operation> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }
        public IResult CheckEmail(string email)
        {
            var getMail = _userDal.Get(u => u.Email == email);
            if (getMail != null)
            {
                return new ErrorResult(Messages.MailExists);
            }
            return new SuccessResult();
        }
        public IResult CheckUserName(string userName)
        {
            var getUser = _userDal.Get(u => u.UserName == userName);
            if (getUser != null)
            {
                return new ErrorResult(Messages.UserNameExists);
            }
            return new SuccessResult();
        }
        public User GetByUserName(string userName)
        {
            return _userDal.Get(u => u.UserName == userName);
        }
        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }
        public IDataResult<List<UserLibraryDto>> GetById(int userId)
        {
            var getUser = _userDal.Get(u => u.Id == userId);
            if (getUser == null)
            {
                return new ErrorDataResult<List<UserLibraryDto>>(Messages.UserNotFound);
            }

            var getPlaylists = _userLibraryDal.GetList(ul => ul.UserId == getUser.Id && ul.Status == true).ToList();
            if (getPlaylists.Count == 0)
            {
                return new ErrorDataResult<List<UserLibraryDto>>(Messages.PlaylistNotFound);
            }

            List<UserLibraryDto> playlists = new();

            foreach (var playlist in getPlaylists)

            {
                UserLibraryDto userLibraryDto = new()
                {
                    Name = playlist.Name,
                    TrackCount = playlist.TrackCount,
                    Type = playlist.Type,
                    PlaylistId = playlist.Id,
                };
                playlists.Add(userLibraryDto);
            }
            return new SuccessDataResult<List<UserLibraryDto>>(playlists);
        }


    }
}

