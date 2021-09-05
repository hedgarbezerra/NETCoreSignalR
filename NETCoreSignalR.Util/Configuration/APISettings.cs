using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Util.Configuration
{
    public class APISettings : BaseSettings
    {
        public APISettings(IConfiguration config)
                 : base(config)
        {
        }

        public string ConnectionString { get => GetConfiguration("DEFAULTCONNECTIONSTR", "ConnectionStrings:SGNR"); }
        public string EncryptionKey { get => GetConfiguration("ENCRYPTIONKEY", "ENCRYPTION_KEY"); }
        public string JWTKey { get => GetConfiguration("TOKENKEY", "TOKEN_KEY"); }
    }
}
