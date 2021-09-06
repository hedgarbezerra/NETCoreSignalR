using Microsoft.Extensions.Configuration;
using Moq;
using NETCoreSignalR.Util.Configuration;
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


        [OneTimeSetUp]
        public void Setup()
        {            
            encryption = new Encryption("l8cpD27QcWDXjAg8ut+qH0IkWv/p38DrAst4Ee83jMg=");
        }

        [Test]
        [TestCase("hello")]
        [TestCase("encryption")]
        [TestCase("module1")]
        public void Encrypt_PlainText_ReturnsDecryptableEncryptedText(string text)
        {
            var encryptedText = encryption.Encrypt(text);
            var decryptedText = encryption.Decrypt(encryptedText);

            Assert.IsNotEmpty(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }
        [Test]
        public void Encrypt_EmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => encryption.Encrypt(string.Empty));
        }

        [Test]
        [TestCase("OMzFW/DobaMAEmQEU4CN7WJ1tSEeT17loULz182L0QY=")]
        [TestCase("2ScKt1hTM3/UAdtj6zahU5y/PF65bjl/BvM7mO0SPkA=")]
        [TestCase("0IJZVN2gsyXb0bcNBA8U7GK+gLIcEK5V2agXC/dCWB8=")]
        public void Decrypt_EncryptedString_DecryptsToTextUnittesting(string text)
        {
            string expectedStringValue = "unittesting";
            var decryptedText = encryption.Decrypt(text);

            Assert.IsNotEmpty(decryptedText);
            Assert.AreEqual(decryptedText, expectedStringValue);

        }
        [Test]
        public void Decrypt_EmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => encryption.Decrypt(string.Empty));

        }
}
}
