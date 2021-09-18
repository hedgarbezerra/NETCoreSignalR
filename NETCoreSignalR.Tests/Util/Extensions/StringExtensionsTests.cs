using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using NETCoreSignalR.Util.Extensions;

namespace NETCoreSignalR.Tests.Util.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {

        [Test]
        public void Mask_StringEmpty_ReturnsSameString()
        {
            string str = string.Empty;
            var emptyMasked = str.Mask(0, 10, 'x');
            emptyMasked.Should().BeEmpty().And.Be(str);
        }

        [Test]
        public void Mask_ToParameterGreaterThanStringLength_ReturnsSameString()
        {
            string str = "oi oi";
            var emptyMasked = str.Mask(0, 10, 'x');
            emptyMasked.Should().NotBeNullOrEmpty().And.Be(str);
        }

        [Test]
        public void Mask_FromParameterGreaterThanStringLength_ReturnsSameString()
        {
            string str = "oi oi";
            var emptyMasked = str.Mask(10, 10, 'x');
            emptyMasked.Should().NotBeNullOrEmpty().And.Be(str);
        }

        [Test]
        public void Mask_ToParameterGreaterThanStringLength_ReturnsMaskedString()
        {
            string str = "oi oi";

            var emptyMasked = str.Mask(3, str.Length, 'x');

            emptyMasked.Should().NotBeEmpty().And.Be("oi xx");
        }

        [Test]
        public void Mask_FromParameterLesserThanStringLengthArgumentOutOfRangeException()
        {
            string str = "oi oi";

            str.Invoking(s => s.Mask(-1, 0, '*')).Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
