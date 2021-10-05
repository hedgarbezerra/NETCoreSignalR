using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IEncryption
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

}
