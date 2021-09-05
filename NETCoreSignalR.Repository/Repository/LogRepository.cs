using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Repository.Repository
{
    public class LogRepository : BaseRepository<EventLog>, IRepository<EventLog>
    {
        public LogRepository(MyDbContext context)
            : base(context)
        {

        }
    }
}
