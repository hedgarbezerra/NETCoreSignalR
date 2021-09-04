using Microsoft.Extensions.Configuration;
using Moq;
using NETCoreSignalR.Util.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Tests.Util.Security
{
    [TestFixture]
    public class EncryptionTest
    {
        private Encryption encryption;
        private Mock<IConfiguration> mqConfig;

        [SetUp]
        public void Setup()
        {
            mqConfig = new Mock<IConfiguration>();
            encryption = new Encryption(mqConfig.Object);
        }
    }
}
