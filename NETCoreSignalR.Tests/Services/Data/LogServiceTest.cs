using Moq;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Repository.Repository;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.Pagination;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NETCoreSignalR.Tests.Services.Data
{
    [TestFixture]
    public class LogServiceTest
    {
        private Mock<IRepository<EventLog>> mqRepository;
        private Mock<IUriService> mqUriService;
        private LogService service;

        [SetUp]
        public void Setup()
        {
            mqRepository = new Mock<IRepository<EventLog>>();
            mqUriService = new Mock<IUriService>();
            service = new LogService(mqRepository.Object, mqUriService.Object);
        }

        [TearDown]
        public void TearDownMocks()
        {
            mqRepository.Reset();
            mqUriService.Reset();
        }

        [Test]
        public void Get_NoFilter_ReturnsEveryLog()
        {
            var list = new List<EventLog>()
            {
                new EventLog("Erro", "NullArgumentException", LogLevel.Warning){Id = 1},
                new EventLog("Erro", "NullArgumentException", LogLevel.Warning){Id = 2},
                new EventLog("Erro", "NullArgumentException", LogLevel.Warning){Id = 3}
            };
            mqRepository.Setup(r => r.Get()).Returns(list.AsQueryable());

            var logs = service.Get();

            Assert.That(logs.Count() == 3);
        }

        [Test]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.Warning)]
        public void Get_FilteredByLogLevelUsingDelegate_ReturnsSingleItemQueryable(LogLevel loglevel)
        {
            var list = new List<EventLog>()
            {
                new EventLog("Erro", "NullArgumentException", LogLevel.Warning){Id = 1},
                new EventLog("Erro", "NullArgumentException", LogLevel.Information){Id = 3},
                new EventLog("Erro", "NullArgumentException", LogLevel.Error){Id = 32}
            };

            Expression<Func<EventLog, bool>> del = (a) => a.LogLevel == loglevel;

            mqRepository.Setup(r => r.Get(It.IsAny<Expression<Func<EventLog, bool>>>())).Returns(list.AsQueryable().Where(del));

            var logs = service.Get(del);

            Assert.That(logs.Count() == 1);
            Assert.IsNotNull(logs.FirstOrDefault());
            Assert.That(logs.FirstOrDefault().LogLevel == loglevel);
        }
        [Test]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.Warning)]
        public void Get_FilteredByLogLevelUsingDelegate_ReturnsEmptyQueryable(LogLevel loglevel)
        {
            var list = new List<EventLog>()
            {
                new EventLog("Erro", "NullArgumentException", LogLevel.Debug){Id = 1},
                new EventLog("Erro", "NullArgumentException", LogLevel.Fatal){Id = 3},
                new EventLog("Erro", "NullArgumentException", LogLevel.Verbose){Id = 32}
            };

            Expression<Func<EventLog, bool>> del = (a) => a.LogLevel == loglevel;

            mqRepository.Setup(r => r.Get(It.IsAny<Expression<Func<EventLog, bool>>>())).Returns(list.AsQueryable().Where(del));

            var logs = service.Get(del);

            Assert.That(logs.Count() == 0);
            Assert.IsNull(logs.FirstOrDefault());
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void Get_FilterLogById_ReturnsEventLog(int id)
        {
            mqRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(new EventLog() { Id = id, LogLevel = LogLevel.Warning, Message = "Erro", CreatedTime = DateTime.UtcNow });
            var logItem = service.Get(id);

            Assert.IsNotNull(logItem);
            Assert.AreEqual(id, logItem.Id);
            Assert.AreNotEqual(logItem.CreatedTime, DateTime.MinValue);
        }

        [Test]
        [TestCase(1342)]
        [TestCase(35)]
        public void Get_FilterLogByNonExistingId_ReturnsNull(int id)
        {
            var logItem = service.Get(id);

            Assert.IsNull(logItem);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Get_FilterLogByInvalidId_ThrowsArgumentException(int id)
        {
            mqRepository.Setup(r => r.Get(It.IsAny<int>())).Throws(new ArgumentException());
            Assert.Throws<ArgumentException>(() => service.Get(id));
        }

        [Test]
        [TestCase("Log", 1, 1)]
        [TestCase("Log", 2, 2)]
        public void GetPaginatedList_PopulatedQueryable_ReturnsPaginatedList(string route, int index, int size)
        {
            var list = new List<EventLog>()
            {
                new EventLog("Erro", "NullArgumentException", LogLevel.Debug){Id = 1},
                new EventLog("Erro", "NullArgumentException", LogLevel.Fatal){Id = 4},
                new EventLog("Erro", "NullArgumentException", LogLevel.Error){Id = 5},
                new EventLog("Erro", "NblablaArgumentException", LogLevel.Fatal){Id = 763},
                new EventLog("Erro", "NullArgumentException", LogLevel.Verbose){Id = 32}
            };

            mqRepository.Setup(r => r.Get()).Returns(list.AsQueryable());

            var paginatedResult = service.GetPaginatedList(route, index, size);

            Assert.AreEqual(paginatedResult.Data.Count, size);
            Assert.AreEqual(paginatedResult.PageSize, size);
            Assert.IsTrue(paginatedResult.HasNextPage);
            Assert.That(paginatedResult.TotalCount == list.Count);
            Assert.That(paginatedResult.TotalPages == (int)Math.Ceiling(paginatedResult.TotalCount / (double)size));

        }
        [Test]
        public void GetPaginatedList_EmptyQueryable_ReturnsPaginatedList()
        {
            var list = new List<EventLog>();

            mqRepository.Setup(r => r.Get()).Returns(list.AsQueryable());
            int index = 1, size = 1;

            var paginatedResult = service.GetPaginatedList("", index, size);

            Assert.AreEqual(paginatedResult.Data.Count, 0);
            Assert.AreEqual(paginatedResult.PageSize, size);
            Assert.That(paginatedResult.TotalCount == list.Count);
            Assert.That(paginatedResult.TotalPages == (int)Math.Ceiling(paginatedResult.TotalCount / (double)size));

        }
    }
}