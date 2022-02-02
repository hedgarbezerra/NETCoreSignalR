using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Interfaces;
using NETCoreSignalR.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Repository.Repository
{
    public class LogRepository : BaseRepository<EventLog>
    {
        public LogRepository(MyDbContext context)
            : base(context)
        {

        }
    }
}
