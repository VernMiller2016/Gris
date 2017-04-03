using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerTimeEntryService
    {
        IEnumerable<ServerTimeEntry> AddServerTimeEntries(IEnumerable<ServerTimeEntry> entities);

        IEnumerable<ServerTimeEntry> GetServerTimeEntries(PagingInfo pagingInfo = null);

        IEnumerable<ServerTimeEntry> GetServerTimeEntries(DateTime selectedDate, PagingInfo pagingInfo = null);

        IEnumerable<ServerTimeEntriesMonthlyReportEntity> GetServerTimeEntriesMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null);
    }
}