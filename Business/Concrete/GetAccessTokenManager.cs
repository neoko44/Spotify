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
        IGetAccessTokenDal _accessTokenDal;

        public GetAccessTokenManager( IGetAccessTokenDal accessTokenDal)
        {
            _accessTokenDal = accessTokenDal;
        }

        public void Add(GetAccessToken getAccessToken)
        {
            _accessTokenDal.Add(getAccessToken);
        }

        public async Task<IDataResult<List<GetAccessToken>>> GetListAsync()
        {
            var getList = _accessTokenDal.GetList().ToList();
            if (getList == null)
            {
                return new ErrorDataResult<List<GetAccessToken>>(Messages.AccessTokenNotFound);
            }
            return new SuccessDataResult<List<GetAccessToken>>(getList);
        }
    }
}
