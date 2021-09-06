using Microsoft.Extensions.Configuration;
using NETCoreSignalR.Util.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NETCoreSignalR.Util.Security
{
    public interface IEncryption
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

    public class Encryption : IEncryption
    {
        public Encryption(string encryptionKey)
        {
            _key = Convert.FromBase64String(encryptionKey);
        }
        private byte[] _key;

        public string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("The value to decrypt can't be null nor empty.");

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
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("The value to encrypt can't be null nor empty.");

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
