using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(int pageIndex, int pageSize, string route);
        Uri GetUri(string route);
    }
}
