using LanguageExt;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface ILoggingService
    {
        IQueryable<EventLog> Get();
        IQueryable<EventLog> Get(Expression<Func<EventLog, bool>> filter);
        Option<EventLog> Get(int id);
        Task<Option<EventLog>> GetAsync(int id, CancellationToken token);
        PaginatedList<EventLog> GetPaginatedList(string route, int pageIndex, int pageSize);

    }
}
