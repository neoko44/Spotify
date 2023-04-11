using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<AccessToken> Register(RegisterDto registerDto);
        List<Operation> GetClaims(User user);
        IResult CheckEmail(string email);
        IResult CheckUserName(string userName);
        IDataResult<AccessToken> CreateAccessToken(User user);


    }
}
