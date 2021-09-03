﻿using Microsoft.Extensions.Configuration;
using NETCoreSignalR.Util.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Util.Configuration
{
    public abstract class BaseSettings
    {
        public BaseSettings(IConfiguration config)
        {
            _config = config;
        }
        public BaseSettings(IConfiguration config, IEncryption encryption)
        {
            _config = config;
            _encryption = encryption;
        }

        protected IConfiguration _config { get; }
        public IEncryption _encryption { get; }

        protected string GetConfiguration(string configKey, string substitutionKey = "", bool shouldDecrypt = false)
        {
            var configValue = _config[configKey] ?? _config[substitutionKey];
            if (shouldDecrypt && _encryption != null)
                configValue = _encryption.Decrypt(configValue ?? "");

            return configValue;
        }

    }
}
