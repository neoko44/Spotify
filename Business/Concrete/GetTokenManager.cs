using Azure;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Business.Concrete
{
    public class GetTokenManager : IGetTokenService
    {
        
        IGetAccessTokenService _accessTokenService;

        public GetTokenManager( IGetAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        public async Task<IDataResult<GetAccessTokenDto>> GetAccessToken()
        {
            var getList = await _accessTokenService.GetListAsync();
            var getLast = getList.Data.Last();

            if (getLast != null)
            {
                if (getLast.CreatedDate.AddHours(1) !> DateTime.Now)
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


            GetAccessTokenDto getAccessTokenDto = new();
            var clientId = "471a760f25484a198e1b8bfe02c63941";
            var clientSecret = "1bfec4d43d134679a780d33875386c81";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", clientId},
            {"client_secret", clientSecret}
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
