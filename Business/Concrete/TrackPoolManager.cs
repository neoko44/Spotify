using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Concrete.JsonEntity;
using Entities.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TrackPoolManager : ITrackPoolService
    {
        IGetTokenService _getTokenService;

        public TrackPoolManager(IGetTokenService getTokenService)
        {
            _getTokenService = getTokenService;
        }

        public async Task<IDataResult<List<TrackDto>>> GetTrackPoolAsync()
        {
            var getAccessToken = await _getTokenService.GetAccessToken();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", getAccessToken.Data.access_token);

            var request = new HttpRequestMessage(HttpMethod.Get, Endpoints.GetPlaylistItems);


            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {

                using (var content = await response.Content.ReadAsStreamAsync())
                {
                    var tracks = await Task.Run(() => JsonConvert.DeserializeObject<Tracks>(new StreamReader(content).ReadToEnd()));
                    if (tracks == null)
                    {
                        return new ErrorDataResult<List<TrackDto>>(Messages.Failed);
                    }

                    List<TrackDto> tracksDto = new();
                    foreach (var item in tracks.items)
                    {
                        List<ArtistDto> artists = new();
                        foreach (var artist in item.track.artists)
                        {
                            ArtistDto artistDto = new()
                            {
                                Id = artist.id,
                                Name = artist.name
                            };
                            artists.Add(artistDto);
                        }
                        var convertToSeconds = item.track.duration_ms / 1000;
                        var TrackTime = TimeSpan.FromSeconds(convertToSeconds).ToString(@"m\:ss");

                        TrackDto trackDto = new()
                        {
                            Id = item.track.id,
                            Name = item.track.name,
                            Duration = TrackTime,
                            Album = new()
                            {
                                Id = item.track.album.id,
                                Name = item.track.album.name,
                                ReleaseDate = item.track.album.release_date
                            },

                            Artists = artists
                        };
                        tracksDto.Add(trackDto);
                    }

                    return new SuccessDataResult<List<TrackDto>>(tracksDto, Messages.PlayListTracksGetSuccess);
                }

            }

            return new ErrorDataResult<List<TrackDto>>(Messages.Failed);

        }
        public async Task<IDataResult<Root>> GetAlbumAsync(string albumId)
        {
            var getAccessToken = await _getTokenService.GetAccessToken();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", getAccessToken.Data.access_token);

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/albums/{albumId}");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var content = await response.Content.ReadAsStreamAsync())
                {
                    var album = await Task.Run(() => JsonConvert.DeserializeObject<Root>(new StreamReader(content).ReadToEnd()));

                    if (album == null)
                    {
                        return new ErrorDataResult<Root>(Messages.Failed);
                    }

                    return new SuccessDataResult<Root>(album, Messages.AlbumGetSuccess);
                }
            }

            return new ErrorDataResult<Root>(Messages.Failed);
        }

    }
}
