﻿using Dawn;
using NETCoreSignalR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NETCoreSignalR.Util.Security
{
    public class Hashing : IHashing
    {
        private readonly RNGCryptoServiceProvider _rngProvider = new RNGCryptoServiceProvider();
        public string ComputeHash(string plainText)
        {

            Guard.Argument(plainText)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();
            byte[] saltBytes = GenerateSaltBytes();

            return ComputeHash(plainText, saltBytes);
        }
        public string ComputeHash(string plainText, byte[] saltBytes = null)
        {

            Guard.Argument(plainText)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();
            if (saltBytes == null || saltBytes.Length <= 0)
            {
                saltBytes = GenerateSaltBytes();
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA256Managed();


            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }

        public bool VerifyHash(string plainText, string hashValue)
        {
            Guard.Argument(plainText)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();

            Guard.Argument(hashValue)
                .NotNull()
                .NotWhiteSpace()
                .NotEmpty();

            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            int hashSizeInBits = 256;
            int hashSizeInBytes;

            hashSizeInBytes = hashSizeInBits / 8;

            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            string expectedHashString = ComputeHash(plainText, saltBytes);

            return hashValue == expectedHashString;
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

        /// <summary>
        /// Generate random string considering chars, numbers and special chars
        /// </summary>
        /// <param name="stringLength">defines the length of the random string</param>
        /// <returns>Generate random string considering chars, numbers and special chars</returns>
        public string RandomString(int stringLength = 8)
        {
            char[] chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%&".ToCharArray();

            byte[] data = new byte[4 * stringLength];

            _rngProvider.GetBytes(data);

            StringBuilder result = new StringBuilder(stringLength);

            for (int i = 0; i < stringLength; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
