using Microsoft.EntityFrameworkCore;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Repository.Configurations;
using NETCoreSignalR.Repository.Repository;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NETCoreSignalR.Tests.Repository
{
    [TestFixture]
    public class LogRepositoryTests
    {
        private MyDbContext _context;
        private LogRepository _repo;

        //Arrange
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            _context = new MyDbContext(options);
            _context.ChangeTracker.LazyLoadingEnabled = false;
            _context.ChangeTracker.Clear();
            _repo = new LogRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.ChangeTracker.Clear();
            _context?.Database.EnsureDeleted();
        }


        [Test]
        public void GetParallel_AllEventLogs_ReturnsEventLogs()
        {
            //act   
            AddLogsToContext();

            var logs = _repo.GetParallel();
            logs.Should().NotBeNullOrEmpty().And.HaveCount(2);
        }

        [Test]
        public void GetParallel_AllEventLogs_EmptyIqueryable()
        {
            var logs = _repo.GetParallel();

            logs.Should().NotBeNull().And.BeEmpty();
        }
        [Test]
        public void GetParallel_FilteredByDelegate_ReturnsFilteredEventLogs()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime < DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.That(logs.Count() >= 2);
        }

        [Test]
        public void GetParallel_FilteredByDelegate_ReturnsEmptyQueryable()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime > DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        public void GetParallel_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsOldestToNewest()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 1);
        }

        [Test]
        public void GetParallel_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsNewestToOldest()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, reverse: true);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 2);
        }

        [Test]
        public void GetParallel_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsNoRecord()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 1, 1, true);
            var userOld = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
            Assert.IsNull(userOld);
        }

        [Test]
        public void GetParallel_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsSingeRecord()
        {
            AddLogsToContext();
            var logs = _repo.GetParallel(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 2, 1, true);
            var log = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(log);
            Assert.That(log.Id == 1);
        }

        [Test]
        public void Get_AllEventLogs_ReturnsEventLogs()
        {
            //act   
            AddLogsToContext();

            var logs = _repo.Get();

            logs.Should().NotBeNullOrEmpty().And.HaveCount(2);
        }

        [Test]
        public void Get_AllEventLogs_EmptyIqueryable()
        {
            var logs = _repo.Get();

            logs.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Get_FilteredById_ReturnsSingleEventLog(int id)
        {
            AddLogsToContext();
            var log = _repo.Get(id);

            log.Should().NotBeNull();
            Assert.AreEqual(log.Id, id);
        }

        [Test]
        [TestCase(5)]
        [TestCase(8)]
        public void Get_FilteredById_ReturnsNull(int id)
        {
            AddLogsToContext();
            var logs = _repo.Get(id);

            Assert.IsNull(logs);
        }

        [Test]
        public void Get_FilteredByInvalidId_ThrowsArgumentException()
        {
            var parameters = new object[0];

            _repo.Invoking(x => x.Get(parameters))
                .Should()
                .Throw<ArgumentException>();
            //Assert.Throws<ArgumentException>(() => _repo.Get(parameters));
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsFilteredEventLogs()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.That(logs.Count() >= 2);
        }

        [Test]
        public void Get_FilteredByDelegate_ReturnsEmptyQueryable()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime > DateTime.Now);

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsOldestToNewest()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 1);
        }

        [Test]
        public void Get_FilteredAndOrderedByRegisteredDate_ReturnsOrderedRecordsNewestToOldest()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, reverse: true);
            var userOld = logs.First();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(userOld);
            Assert.AreEqual(userOld.Id, 2);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsNoRecord()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 1, 1, true);
            var userOld = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
            Assert.IsNull(userOld);
        }

        [Test]
        public void Get_FilteredAndOrderedWithLimitAndSkippedRecords_ReturnsSingeRecord()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime < DateTime.Now, c => c.CreatedTime, 2, 1, true);
            var log = logs.FirstOrDefault();

            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.IsNotNull(log);
            Assert.That(log.Id == 1);
        }

        [Test]
        public void Get_FilteredAndOrderedWithNoMatches_ReturnsEmpty()
        {
            AddLogsToContext();
            var logs = _repo.Get(x => x.CreatedTime > DateTime.Now, c => c.CreatedTime);

            Assert.IsNotNull(logs);
            Assert.IsEmpty(logs);
        }

        [Test]
        public async Task GetAsync_FilterById_ReturnsSingleLog()
        {
            AddLogsToContext();

            var cTokenSource = new CancellationTokenSource(500);
            var log = await _repo.GetAsync(cTokenSource.Token, 1);

            log.Should().NotBeNull();
            log.Id.Should().Be(1);
            log.CreatedTime.Should().Be(new DateTime(2021, 5, 10));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_DeletingEventLogs_EventLogDeletedFromContext(int id)
        {
            AddLogsToContext();

            var log = _repo.Get(id);
            _repo.Delete(log);
            _repo.SaveChanges();

            Assert.That(!_context.Logs.Any(x => x.Id == id));
        }

        [Test]
        public void GetDbContext_ReturnsCurrentDbContext()
        {
            var dbContext = _repo.GetDbContext();

            Assert.IsNotNull(dbContext);
        }

        [Test]
        public void Add_EventLogAdded()
        {
            var log = new EventLog { Message = "New log", Exception = "roleEventLog", CreatedTime = new DateTime(2021, 08, 29) };
            var addedEventLog = _repo.Add(log);
            _repo.SaveChanges();

            var logs = _repo.Get();

            Assert.That(logs.Count() == 1);
            Assert.AreEqual(logs.First().CreatedTime, addedEventLog.CreatedTime);
            Assert.AreEqual(logs.First().Message, addedEventLog.Message);
        }

        [Test]
        public async Task AddAsync_EventLogAdded()
        {
            var log = new EventLog { Message = "New log", Exception = "roleEventLog", CreatedTime = new DateTime(2021, 08, 29) };
            var addedEventLog = await _repo.AddAsync(log);
            _repo.SaveChanges();

            var logs = _repo.Get();

            Assert.That(logs.Count() == 1);
            Assert.AreEqual(logs.First().CreatedTime, addedEventLog.CreatedTime);
            Assert.AreEqual(logs.First().Message, addedEventLog.Message);
        }

        [Test]
        public void Update_EventLogUpdated()
        {
            //arrange
            AddLogsToContext();
            var toUpdatelog = _repo.Get().FirstOrDefault();
            toUpdatelog.Message = "New log";
            toUpdatelog.Exception = "roleEventLog";
            toUpdatelog.CreatedTime = new DateTime(2021, 08, 29);

            //act
            var updatedLog = _repo.Update(toUpdatelog);

            var dbLog = _repo.Get(updatedLog.Id);

            //assert
            updatedLog.Should().NotBeNull().And.Be(dbLog);
        }

        private async Task AddLogsToContext()
        {
            _context.Logs.Add(new EventLog { Id = 1, Message = "Error 1", CreatedTime = new DateTime(2021, 5, 10) });
            _context.Logs.Add(new EventLog { Id = 2, Message = "Error 2", CreatedTime = new DateTime(2021, 8, 10) });
            await _context.SaveChangesAsync();
        }
    }
}
