using NETCoreSignalR.Util.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NETCoreSignalR.Tests.Util.Security
{
    
    [TestFixture]
    public class HashingTest
    {
        private Hashing _hashing;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _hashing = new Hashing();
        }

        [Test]
        public void ComputeHash()
        {
            string plainText = "hashthis";

            var hashedText = _hashing.ComputeHash(plainText);

            Assert.IsNotNull(hashedText);
            Assert.IsNotEmpty(hashedText);
            Assert.IsTrue(_hashing.VerifyHash(plainText, hashedText));
        }

        [Test]
        public void ComputeHash_EmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _hashing.ComputeHash(""));
        }

        [Test]
        public void ComputeHashWithSaltyBytes_EmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _hashing.ComputeHash("", null));
        }

        [Test]
        [TestCase("hashthat", "JpgVgabQhXcpy38k7OrUOR5sBj/PO/AS/CDRjsE55oxrwAXE0ig8")]
        [TestCase("hashthis", "hFxsrh/4e3/Out4C1dwz+xmcmk+PKOpCHk7SSfGOR1ucNBodNi8=")]
        public void VerifyHash(string plainText, string hash)
        {
            Assert.IsTrue(_hashing.VerifyHash(plainText, hash));
        }

        [Test]
        public void VerifyHash_TextAndHashEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _hashing.VerifyHash("", ""));
        }

        [Test]
        public void VerifyHash_EmptyText_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _hashing.VerifyHash("", "340Q230Y489IQJEW"));
        }
        [Test]
        public void VerifyHash_EmptyHashValue_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _hashing.VerifyHash("hashhash", ""));
        }

        private byte[] GenerateSaltBytes()
        {
            RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
            int minSaltSize = 4;
            int maxSaltSize = 8;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            var saltBytes = new byte[saltSize];


            rngProvider.GetNonZeroBytes(saltBytes);

            return saltBytes;
        }
    }   
}
