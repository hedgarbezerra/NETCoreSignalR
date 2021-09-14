using AutoFixture;
using Moq;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Services.Pagination;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCoreSignalR.Tests.Services.Pagination
{
    
    [TestFixture]
    public class PaginatedListTest
    {
        private MockRepository mqRepository = new MockRepository(MockBehavior.Default);
        private Fixture _fixture = new Fixture();

        [Test]
        [TestCase(0, 5)]
        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(2, 2)]
        public void ConstructPaginatedList_Scenary_Expect(int pageIndex, int pageSize)
        {
            //arrange
            var fixture = new Fixture();
            var query = fixture.Build<EventLog>().CreateMany(15).AsQueryable();
            var route = "/test";
            var baseUrl = "http://localhost.com";
            var mqUriService = mqRepository.Of<IUriService>()
                .Where(u => u.GetPageUri(pageIndex - 1, pageSize, route) == new Uri(string.Concat(baseUrl, route, "?previous")))
                .Where(u => u.GetPageUri(pageIndex + 1, pageSize, route) == new Uri(string.Concat(baseUrl, route, "?next")))
                .First();

            //act
            var paginatedList = new PaginatedList<EventLog>(query, mqUriService, route, pageIndex, pageSize);

            //act

            Assert.IsNotEmpty(paginatedList.Data);
            Assert.IsTrue(paginatedList.TotalCount == query.Count());
            Assert.IsTrue(paginatedList.Data.Count == pageSize);
            Assert.AreEqual(paginatedList.Data, query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            Assert.That((int)Math.Ceiling(query.Count() / (double)pageSize) == paginatedList.TotalPages);
            mqRepository.Verify();
        }
    }
}
