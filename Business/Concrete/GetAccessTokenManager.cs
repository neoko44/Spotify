using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GetAccessTokenManager : IGetAccessTokenService
    {
        IGetAccessTokenService _accessTokenService;
        IGetAccessTokenDal _accessTokenDal;

        public GetAccessTokenManager(IGetAccessTokenService accessTokenService, IGetAccessTokenDal accessTokenDal)
        {
            _accessTokenService = accessTokenService;
            _accessTokenDal = accessTokenDal;
        }

        public void Add(GetAccessToken getAccessToken)
        {
            _accessTokenService.Add(getAccessToken);
        }

        public IDataResult<GetAccessToken> GetLast()
        {
            var getLast = _accessTokenDal.GetList().ToList().Last();
            if (getLast == null)
            {
                return new ErrorDataResult<GetAccessToken>(Messages.AccessTokenNotFound);
            }
            return new SuccessDataResult<GetAccessToken>(getLast);
        }
    }
}
