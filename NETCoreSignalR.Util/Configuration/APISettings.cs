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

        public string ConnectionString { get => GetConfiguration("SGNR-CONNECTION-STR", "ConnectionStrings:SGNR"); }
        public string EncryptionKey { get => GetConfiguration("ENCRYPTION-KEY", "ENCRYPTION_KEY"); }
        public string JWTKey { get => GetConfiguration("TOKEN-KEY", "TOKEN_KEY"); }
        public string KeyVaultClientId { get => GetConfiguration("AZR_KV_APP_ID"); }
        public string KeyVaultURI { get => GetConfiguration("AZR_KV_URI"); }
        public string KeyVaultKey { get => GetConfiguration("AZR_KV_KEY"); }
    }
}
