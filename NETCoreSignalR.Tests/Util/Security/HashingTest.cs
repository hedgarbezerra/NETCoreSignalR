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
        public void ComputeHash_ValidHash_ReturnHashedString()
        {
            string plainText = "hashthis";

            var hashedText = _hashing.ComputeHash(plainText);

            Assert.IsNotNull(hashedText);
            Assert.IsNotEmpty(hashedText);
            Assert.IsTrue(_hashing.VerifyHash(plainText, hashedText));
        }

        [Test]
        public void ComputeHash_EmptySaltBytes_GeneratesSaltAndReturnHashedString()
        {
            string plainText = "hashthis";
            byte[] saltBytes = new byte[0];
            var hashedText = _hashing.ComputeHash(plainText, saltBytes);

            Assert.IsNotNull(hashedText);
            Assert.IsNotEmpty(hashedText);
            Assert.IsTrue(_hashing.VerifyHash(plainText, hashedText));
        }

        [Test]
        public void ComputeHash_NullSaltBytes_GeneratesSaltAndReturnHashedString()
        {
            string plainText = "hashthis";

            var hashedText = _hashing.ComputeHash(plainText, null);

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
        public void VerifyHash_expectedHashAndPlainText_ReturnsTrue(string plainText, string hash)
        {
            Assert.IsTrue(_hashing.VerifyHash(plainText, hash));
        }

        [Test]
        [TestCase("b2lvaW9pcXdlcXdl")]
        [TestCase("cWl3dWVvcXdkanF3")]
        public void VerifyHash_DifferentString_ReturnsFalse(string hash)
        {
            Assert.IsFalse(_hashing.VerifyHash("any", hash));
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
    }   
}
