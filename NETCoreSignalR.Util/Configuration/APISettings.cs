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

        public string ConnectionString { get => GetConfiguration("DEFAULTCONNECTIONSTR", "ConnectionStrings:DEFAULT"); }
        public string EncryptionKey { get => GetConfiguration("ENCRYPTIONKEY", "ENCRYPTION_KEY"); }
    }
}
