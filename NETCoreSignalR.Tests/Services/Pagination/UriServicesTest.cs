using AutoFixture;
using Microsoft.AspNetCore.WebUtilities;
using NETCoreSignalR.Services.Pagination;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace NETCoreSignalR.Tests.Services.Pagination
{
    [TestFixture]
    public class UriServiceTests
    {
        private UriService _uriService;
        private const string _baseUrl = "http://localhost.com";
        static readonly List<object> GetUriTestCaseRoute = new List<object>
        {
            new object[] { "/test/get", _baseUrl+ "/test/get" },
            new object[] { "/test/post", _baseUrl+ "/test/post" },
            new object[] { "/test/delete", _baseUrl+ "/test/delete" },
        };


        [SetUp]
        public void SetUp()
        {
            _uriService = new UriService(_baseUrl);
        }

        [Test]
        [TestCase("http://localhost.com")]
        [TestCase("http://randomapi.com")]
        public void UriService_Constructing_ValidBaseUri(string uri)
        {
            var uriService = new UriService(uri);

            Assert.IsNotNull(uriService);
            Assert.IsInstanceOf(typeof(UriService), uriService);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        public void UriService_Constructing_EmptyBaseUri(string baseURI)
        {
            Assert.Throws<ArgumentException>(() => new UriService(baseURI));
        }
        [Test]
        public void UriService_Constructing_nullBaseUri()
        {
            Assert.Throws<ArgumentNullException>(() => new UriService(null));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 5)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        public void GetPageUri_StateUnderTest_ExpectedBehavior(int pageIndex, int pageSize)
        {
            // Arrange
            string route = "/test/get";

            // Act
            var result = _uriService.GetPageUri(pageIndex, pageSize, route);
            var queryParams = HttpUtility.ParseQueryString(result.Query);
            var pageSizeQPM = queryParams.Get("pageSize");
            var pageIndexQPM = queryParams.Get("pageIndex");

            // Assert
            Assert.IsNotNull(queryParams);
            Assert.IsNotNull(pageIndexQPM);
            Assert.IsNotNull(pageSizeQPM);
            Assert.That(() => {
                var pgSizeQPM = Convert.ToInt32(pageSizeQPM);  
                return pgSizeQPM == pageSize;
            });
            Assert.That(() => {
                var pgIndexQPM = Convert.ToInt32(pageIndexQPM);

                return pageIndex == pgIndexQPM;
            });
        }


        [Test]
        [TestCaseSource(nameof(GetUriTestCaseRoute))]
        public void GetUri_StateUnderTest_ExpectedBehavior(string route, string expectedUriString)
        {
            // Act
            var result = _uriService.GetUri(route);
            var resultString = result.AbsoluteUri;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(resultString);
            Assert.AreEqual(resultString, expectedUriString);
        }
    }
}
