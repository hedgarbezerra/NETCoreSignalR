using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Util.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string value, int from, int to, char substitution)
        {
            if (value.Length < to || value.Length < from)
                return value;

            return string.Create(value.Length, value, (span, str) => 
            {
                value.AsSpan().CopyTo(span);
                span[from..to].Fill(substitution);
            });  
        }
    }
}
