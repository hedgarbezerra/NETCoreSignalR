using Microsoft.Extensions.Configuration;
using NETCoreSignalR.Domain.Model;
using NETCoreSignalR.Util.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Services.External
{
    public interface ISpotifyService
    {
        List<SpotifyMusic> GetFavoriteMusics(string userId);
        SpotifyData GetSpotifyData(string userId);
    }

    public class SpotifyService : ISpotifyService
    {
        public IHttpConsumer _httpConsumer { get; }
        public SpotifySettings _spotifySettings { get; }

        public SpotifyService(IHttpConsumer httpConsumer, IConfiguration config)
        {
            _httpConsumer = httpConsumer;
            _spotifySettings = new SpotifySettings(config);
        }
        public SpotifyData GetSpotifyData(string userId)
        {
            //Get User by Id from DB
            //Lookup in Spotify API for user data by using user field for SpotifyID

            var result = _httpConsumer.Get<SpotifyData>("");

            return result;
        }

        public List<SpotifyMusic> GetFavoriteMusics(string userId)
        {

            //get
            return new List<SpotifyMusic>();

        }
    }
}
