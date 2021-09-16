using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Util.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string value, int from, int to, char substitution)
        {
            return string.Create(value.Length, value, (span, str) => 
            {
                value.AsSpan().CopyTo(span);
                span[from..to].Fill(substitution);
            });  
        }

        public static string EnumToString(this Enum @enum) => nameof(@enum);
    }
}
