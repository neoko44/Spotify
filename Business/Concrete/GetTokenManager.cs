using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.JsonEntity;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;

namespace Business.Concrete
{
    public class GetTokenManager : IGetTokenService
    {

        IGetAccessTokenService _accessTokenService;
        private readonly string _clientId, _clientSecret;

        public GetTokenManager(IGetAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
            _clientId = "471a760f25484a198e1b8bfe02c63941";
            _clientSecret = "1bfec4d43d134679a780d33875386c81";
        }

        public string ClientId { get { return _clientId; } }
        public string ClientSecret { get { return _clientSecret; } }
        public async Task<IDataResult<GetAccessTokenDto>> GetAccessToken()
        {
            //var a = _tokenHelper.GetTokenInfo(); // kullanıcının tokeni decrypt ediliyor ve buradan sonra gerçek kişi olup olmadığı doğrulanacak

            var getList = await _accessTokenService.GetListAsync();
            if (getList.Data.Count != 0)
            {
                var getLast = getList.Data.Last();

                if (getLast != null)
                {
                    if (getLast.CreatedDate.Value.AddHours(1)! > DateTime.Now)
                    {
                        GetAccessTokenDto tempGetAccessTokenDto = new()
                        {
                            access_token = getLast.access_token,
                            expires_in = getLast.expires_in,
                            token_type = getLast.token_type
                        };

                        return new SuccessDataResult<GetAccessTokenDto>(tempGetAccessTokenDto);
                    }
                }
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", ClientId},
            {"client_secret", ClientSecret}
        });

            var response = await client.SendAsync(request);
            var responseContent = JsonConvert.DeserializeObject<GetAccessTokenDto>(response.Content.ReadAsStringAsync().Result);


            GetAccessToken accessToken = new()
            {
                access_token = responseContent.access_token,
                token_type = responseContent.token_type,
                expires_in = responseContent.expires_in,
                CreatedDate = DateTime.Now
            };

            _accessTokenService.Add(accessToken);
            return new SuccessDataResult<GetAccessTokenDto>(responseContent);
        }

    }
}
