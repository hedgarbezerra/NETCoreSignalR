using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NETCoreSignalR.Domain.Model
{
    [ExcludeFromCodeCoverage]
    public class PaginationFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
