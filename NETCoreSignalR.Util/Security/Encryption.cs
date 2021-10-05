using Dawn;
using Microsoft.Extensions.Configuration;
using NETCoreSignalR.Domain.Interfaces;
using NETCoreSignalR.Util.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NETCoreSignalR.Util.Security
{
    public class Encryption : IEncryption
    {
        public Encryption(string encryptionKey)
        {
            _key = Convert.FromBase64String(encryptionKey);
        }
        private byte[] _key;

        public string Decrypt(string value)
        {
            Guard.Argument(value)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();

            var ivAndCipherText = Convert.FromBase64String(value);
            using var aes = Aes.Create();
            var ivLength = aes.BlockSize / 8;
            aes.IV = ivAndCipherText.Take(ivLength).ToArray();
            aes.Key = _key;
            using var cipher = aes.CreateDecryptor();
            var cipherText = ivAndCipherText.Skip(ivLength).ToArray();
            var text = cipher.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(text);
        }

        public string Encrypt(string value)
        {
            Guard.Argument(value)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();
            
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();
            using var cipher = aes.CreateEncryptor();
            var text = Encoding.UTF8.GetBytes(value);
            var cipherText = cipher.TransformFinalBlock(text, 0, text.Length);

            return Convert.ToBase64String(aes.IV.Concat(cipherText).ToArray());
        }
    }
}
