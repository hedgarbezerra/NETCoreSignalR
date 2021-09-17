using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Repository.Repository;
using NETCoreSignalR.Services.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Services.Data
{
    public interface ILoggingService
    {
        IQueryable<EventLog> Get();
        IQueryable<EventLog> Get(Expression<Func<EventLog, bool>> filter);
        EventLog Get(int id);
        Task<EventLog> GetAsync(int id, CancellationToken token);
        PaginatedList<EventLog> GetPaginatedList(string route, int pageIndex, int pageSize);

    }
    public class LogService : ILoggingService
    {
        public LogService(IRepository<EventLog> repository, IUriService uriService)
        {
            _repository = repository;
            _uriService = uriService;
        }

        public IRepository<EventLog> _repository { get; }
        public IUriService _uriService { get; }


        public IQueryable<EventLog> Get() => _repository.Get();

        public IQueryable<EventLog> Get(Expression<Func<EventLog, bool>> filter) => _repository.Get(filter);

        public EventLog Get(int id) => _repository.Get(id);

        public async Task<EventLog> GetAsync(int id, CancellationToken cancellationToken) => await _repository.GetAsync(cancellationToken, id);

        public PaginatedList<EventLog> GetPaginatedList(string route, int pageIndex, int pageSize) =>
            new PaginatedList<EventLog>(_repository.Get(), _uriService, route, pageIndex, pageSize);
    }
}
