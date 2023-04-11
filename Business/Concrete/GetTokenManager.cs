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
        private string _token;
        IGetAccessTokenService _accessTokenService;


        public GetTokenManager(string token, IGetAccessTokenService accessTokenService)
        {
            _token = token;
            _accessTokenService = accessTokenService;
        }

        public async Task<IDataResult<GetAccessTokenDto>> GetAccessToken()
        {
            var getLast = _accessTokenService.GetLast();
            if (getLast != null)
            {
                if (getLast.Data.CreatedDate.AddHours(1) > DateTime.Now)
                {
                    GetAccessTokenDto tempGetAccessTokenDto = new()
                    {
                        access_token = getLast.Data.access_token,
                        expires_in = getLast.Data.expires_in,
                        token_type = getLast.Data.token_type
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
            _token = getAccessTokenDto.access_token;

            GetAccessToken accessToken = new()
            {
                access_token = getAccessTokenDto.access_token,
                token_type = getAccessTokenDto.token_type,
                expires_in = getAccessTokenDto.expires_in,
                CreatedDate = DateTime.Now
            };

            _accessTokenService.Add(accessToken);
            return new SuccessDataResult<GetAccessTokenDto>(responseContent);
        }
    }
}
