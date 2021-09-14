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
    public class SpotifySettingsTest
    {
        private MockRepository _mockRepository;

        private SpotifySettings _settings;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
        }

        [Test]
        public void GetClientId_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["SPTFY-CLIENT-ID"]).Returns("kvstring");

            Assert.IsNotNull(_settings.ClientId);
            Assert.IsNotEmpty(_settings.ClientId);
            Assert.AreEqual(_settings.ClientId, "kvstring");
            _mockRepository.Verify();
        }

        [Test]
        public void GetClientId_KeyNotFound_ReturnsSubstitutionKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["SPTFY_CLIENT_ID"]).Returns("environmentvariable");

            Assert.IsNotNull(_settings.ClientId);
            Assert.IsNotEmpty(_settings.ClientId);
            Assert.AreEqual(_settings.ClientId, "environmentvariable");
            _mockRepository.Verify();
        }

        [Test]
        public void GetClientId_KeyNorSubstitutionNotFound_ReturnsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            Assert.IsNull(_settings.ClientId);
            _mockRepository.Verify();
        }


        [Test]
        public void GetClientSecret_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["SPTFY-CLIENT-SECRET"]).Returns("kvstring");

            Assert.IsNotNull(_settings.ClientSecret);
            Assert.IsNotEmpty(_settings.ClientSecret);
            Assert.AreEqual(_settings.ClientSecret, "kvstring");
            _mockRepository.Verify();
        }

        [Test]
        public void GetClientSecret_KeyNotFound_ReturnsSubstitutionKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["SPTFY_CLIENT_SECRET"]).Returns("environmentvariable");

            Assert.IsNotNull(_settings.ClientSecret);
            Assert.IsNotEmpty(_settings.ClientSecret);
            Assert.AreEqual(_settings.ClientSecret, "environmentvariable");
            _mockRepository.Verify();
        }

        [Test]
        public void GetClientSecret_KeyNorSubstitutionNotFound_ReturnsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _settings = new SpotifySettings(mqConfiguration.Object);

            Assert.IsNull(_settings.ClientSecret);
            _mockRepository.Verify();
        }
    }
}
