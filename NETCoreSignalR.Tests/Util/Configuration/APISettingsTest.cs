using Microsoft.Extensions.Configuration;
using Moq;
using NETCoreSignalR.Util.Configuration;
using NETCoreSignalR.Util.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Tests.Util.Configuration
{
    [TestFixture]
    public class APISettingsTest
    {
        private MockRepository _mockRepository;
        
        private APISettings _apiSettings;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
        }
        [Test]
        public void GetJWTKeyEncryption_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["TOKEN-KEY"]).Returns("anystring");

            Assert.IsNotNull(_apiSettings.JWTKey);
            Assert.IsNotEmpty(_apiSettings.JWTKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetJWTKeyEncryption_KeyNotFound_ReturnsSubstitutionKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);

            mqConfiguration.SetupGet(a => a["TOKEN_KEY"]).Returns("anystring");

            Assert.IsNotNull(_apiSettings.JWTKey);
            Assert.IsNotEmpty(_apiSettings.JWTKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetJWTKeyEncryption_KeyNorSubstitutionNotFound_ReturnsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNull(_apiSettings.JWTKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetEncryptionKey_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            mqConfiguration.SetupGet(a => a["ENCRYPTION-KEY"]).Returns("otherstring");

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNotNull(_apiSettings.EncryptionKey);
            Assert.IsNotEmpty(_apiSettings.EncryptionKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetEncryptionKey_KeyNotFound_ReturnsSubstitutionKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            mqConfiguration.SetupGet(a => a["ENCRYPTION_KEY"]).Returns("otherstring");

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNotNull(_apiSettings.EncryptionKey);
            Assert.IsNotEmpty(_apiSettings.EncryptionKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetEncryptionKey_NoKeyMatches_KeyIsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNull(_apiSettings.EncryptionKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetConnectionString_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
            mqConfiguration.SetupGet(a => a["SGNR-CONNECTION-STR"]).Returns("justanotherstring");
            Assert.IsNotNull(_apiSettings.ConnectionString);
            Assert.IsNotEmpty(_apiSettings.ConnectionString);
        }
        [Test]
        public void GetConnectionString_KeyNotFound_ReturnsSubstitutionKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
            mqConfiguration.SetupGet(a => a["ConnectionStrings:SGNR"]).Returns("justanotherstring");
            Assert.IsNotNull(_apiSettings.ConnectionString);
            Assert.IsNotEmpty(_apiSettings.ConnectionString);
        }

        [Test]
        public void GetConnectionString_NoKeyMatches_KeyIsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNull(_apiSettings.ConnectionString);
        }
               
        [Test]
        public void GetKVClientId_NoKeyMatches_KeyIsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNull(_apiSettings.KeyVaultClientId);
        }

        [Test]
        public void GetKVClientId_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
            mqConfiguration.SetupGet(a => a["AZR_KV_APP_ID"]).Returns("keyvaultappId");


            Assert.IsNotNull(_apiSettings.KeyVaultClientId);
            Assert.IsNotEmpty(_apiSettings.KeyVaultClientId);
            _mockRepository.Verify();
        }

        [Test]
        public void GetKVKey_NoKeyMatches_KeyIsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
            Assert.IsNull(_apiSettings.KeyVaultKey);
        }
        [Test]
        public void GetKVCKey_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            mqConfiguration.SetupGet(a => a["AZR_KV_KEY"]).Returns("kvKey");

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNotNull(_apiSettings.KeyVaultKey);
            Assert.IsNotEmpty(_apiSettings.KeyVaultKey);
            _mockRepository.Verify();
        }

        [Test]
        public void GetKVURI_KeyFound_ReturnsKey()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            mqConfiguration.SetupGet(a => a["AZR_KV_URI"]).Returns("azureKvURI");

            _apiSettings = new APISettings(mqConfiguration.Object);

            Assert.IsNotNull(_apiSettings.KeyVaultURI);
            Assert.IsNotEmpty(_apiSettings.KeyVaultURI);
            _mockRepository.Verify();
        }

        [Test]
        public void GetKVURI_NoKeyMatches_KeyIsNull()
        {
            var mqConfiguration = _mockRepository.Create<IConfiguration>();
            _apiSettings = new APISettings(mqConfiguration.Object);
            Assert.IsNull(_apiSettings.KeyVaultURI);
        }
    }
}
