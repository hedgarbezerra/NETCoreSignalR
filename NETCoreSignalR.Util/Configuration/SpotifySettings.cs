using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Util.Configuration
{
    public class SpotifySettings : BaseSettings
    {
        public SpotifySettings(IConfiguration config) : base(config)
        {
        }
        public string ClientId { get => GetConfiguration("SPTFY-CLIENT-ID", "SPTFY_CLIENT_ID"); }
        public string ClientSecret { get => GetConfiguration("SPTFY-CLIENT-SECRET", "SPTFY_CLIENT_SECRET"); }
    }
}
