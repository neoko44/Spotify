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
        IUserOperationDal _operationDal;

        public UserManager(IUserDal userDal, ITokenHelper tokenHelper, IUserOperationDal operationDal)
        {
            _userDal = userDal;
            _tokenHelper = tokenHelper;
            _operationDal = operationDal;
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

            UserOperation userOperation = new()
            {
                OperationId = 1,
                UserId = user.Id
            };

            _operationDal.Add(userOperation);


            var accessToken = CreateAccessToken(user);
            if(accessToken == null)
            {
                return new ErrorDataResult<AccessToken>(Messages.AccessTokenError);
            }

            return new SuccessDataResult<AccessToken>(accessToken.Data,Messages.UserRegistered.ToString());
        }

    }
}
