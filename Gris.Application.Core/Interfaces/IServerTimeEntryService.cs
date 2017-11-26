using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerTimeEntryService
    {
        ServerTimeEntry GetById(int id);

        bool TimeEntryExists(ServerTimeEntry entity);

        void AddServerTimeEntry(ServerTimeEntry entity);

        void AddServerTimeEntries(IEnumerable<ServerTimeEntry> entities);

        IEnumerable<ServerTimeEntry> GetServerTimeEntries(DateTime? selectedDate, string serverName, string paysourceName, PagingInfo pagingInfo = null);

        IEnumerable<ServerTimeEntriesMonthlyReportEntity> GetServerTimeEntriesMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null);

        void UpdateServerTimeEntry(ServerTimeEntry entity, bool applyProgramEditToAllEntries = false);

        void Remove(ServerTimeEntry entity);
    }
}