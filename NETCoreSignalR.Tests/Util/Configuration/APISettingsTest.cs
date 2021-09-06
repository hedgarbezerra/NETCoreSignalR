using Microsoft.Extensions.Configuration;
using Moq;
using NETCoreSignalR.Util.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Tests.Util.Configuration
{
    [TestFixture]
    public class APISettingsTest
    {
        private Mock<IConfiguration> mqConfiguration;
        private APISettings _apiSettings;

        [OneTimeSetUp]
        public void Setup()
        {
            mqConfiguration = new Mock<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
        }

        [Test]
        public void GetJWTKey()
        {
            mqConfiguration.SetupGet(a => a[It.IsAny<string>()]).Returns("");
        }

        [Test]
        public void GetEncryptionKey()
        {
            mqConfiguration.SetupGet(a => a[It.IsAny<string>()]).Returns("");

        }

        [Test]
        public void GetConnectionString()
        {
            mqConfiguration.SetupGet(a => a[It.IsAny<string>()]).Returns("");

        }
    }
}
