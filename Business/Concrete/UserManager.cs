using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        ITokenHelper _tokenHelper;

        public UserManager(IUserDal userDal, ITokenHelper tokenHelper)
        {
            _userDal = userDal;
            _tokenHelper = tokenHelper;
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

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userDal.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public List<Operation> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public IDataResult<User> Register(RegisterDto registerDto)
        {
            if (registerDto.UserName == null || registerDto.UserName == "")
            {
                return new ErrorDataResult<User>(Messages.UserNameNull);
            }
            if (registerDto.Password == null || registerDto.Password == "")
            {
                return new ErrorDataResult<User>(Messages.PasswordNull);
            }
            if (registerDto.Email == null || registerDto.Email == "")
            {
                return new ErrorDataResult<User>(Messages.EmailNull);
            }
            if (registerDto.FirstName == null || registerDto.FirstName == "")
            {
                return new ErrorDataResult<User>(Messages.FirstNameNull);
            }
            if (registerDto.LastName == null || registerDto.LastName == "")
            {
                return new ErrorDataResult<User>(Messages.LastNameNull);
            }
            if (!CheckEmail(registerDto.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.MailExists);
            }
            if (!CheckUserName(registerDto.UserName).Success)
            {
                return new ErrorDataResult<User>(Messages.UserNameExists);
            }


            byte[] passwordSalt, passwordHash;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordSalt, out passwordHash);

            User user = new()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                RoleId = 1,
                Status = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _userDal.Add(user);

            var accessToken = CreateAccessToken(user);
            return new SuccessDataResult<User>(user,Messages.UserRegistered.ToString());
        }

    }
}
