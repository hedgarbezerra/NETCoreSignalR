using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IHashing
    {
        string ComputeHash(string plainText, byte[] saltBytes = null);
        string ComputeHash(string plainText);
        bool VerifyHash(string plainText, string hashValue);
    }
}
