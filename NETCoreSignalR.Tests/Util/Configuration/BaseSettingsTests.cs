using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NETCoreSignalR.Util.Configuration;
using NETCoreSignalR.Util.Security;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Tests.Util.Configuration
{
    internal class MockBaseSettings : BaseSettings
    {
        internal MockBaseSettings(IConfiguration config) : base(config)
        {
        }
        internal MockBaseSettings(IConfiguration config, IEncryption encryption) : base(config, encryption)
        {
        }

        public string GetConfig(string key, string keySub, bool decrypt) => GetConfiguration(key, keySub, decrypt);
    }

    [TestFixture]
    public class BaseSettingsTests
    {
        private MockRepository _mockRepository;
        private Fixture _fixture;
        private Mock<IEncryption> _mqEncryption;
        private Mock<IConfiguration> _mqConfig;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Default);
            _mqEncryption = _mockRepository.Create<IEncryption>();
            _mqConfig = _mockRepository.Create<IConfiguration>();
        }

        [Test]
        public void GetConfiguration_ExistingEncryptedKeyShouldDecrypt_ReturnsDecryptedString()
        {
            var key = "a";
            var keySub = "b";
            var encryptionResult = _fixture.Create<string>();
            var keyFoundValue = _fixture.Create<string>();
            _mqConfig.Setup(x => x[key])
                .Returns(keyFoundValue);
            _mqEncryption.Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns(encryptionResult);

            var settings = new MockBaseSettings(_mqConfig.Object, _mqEncryption.Object);
            var value = settings.GetConfig(key, keySub, true);

            value.Should().NotBeNullOrEmpty().And.BeEquivalentTo(encryptionResult);
        }

        [Test]
        public void GetConfiguration_KeyNotFoundSubFound_ReturnsDecryptedString()
        {
            var key = "a";
            var keySub = "b";
            var encryptionResult = _fixture.Create<string>();
            var keyFoundValue = _fixture.Create<string>();
            _mqConfig.Setup(x => x[keySub])
                .Returns(keyFoundValue);
            _mqEncryption.Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns(encryptionResult);

            var settings = new MockBaseSettings(_mqConfig.Object, _mqEncryption.Object);
            var value = settings.GetConfig(key, keySub, true);

            value.Should().NotBeNullOrEmpty().And.BeEquivalentTo(encryptionResult);
        }

        [Test]
        public void GetConfiguration_NeitherTheKeyOrSubstitutionFound_ReturnsNull()
        {
            var key = "a";
            var keySub = "b";

            var settings = new MockBaseSettings(_mqConfig.Object, _mqEncryption.Object);
            var value = settings.GetConfig(key, keySub, true);

            value.Should().BeNull();
        }

        [Test]
        public void GetConfiguration_ExistingKey_ReturnsKeyValue()
        {
            var key = "a";
            var keySub = "b";
            var keyFoundValue = _fixture.Create<string>();
            _mqConfig.Setup(x => x[key])
                .Returns(keyFoundValue);

            var settings = new MockBaseSettings(_mqConfig.Object, _mqEncryption.Object);
            var value = settings.GetConfig(key, keySub, false);

            value.Should().NotBeNullOrEmpty().And.BeEquivalentTo(keyFoundValue);
        }
    }
}
