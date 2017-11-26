using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IServerTimeEntryRepository : IRepository<ServerTimeEntry>
    {
        List<ServerTimeEntry> SearchForEntries(DateTime? selectedDate, string serverName, string paysourceName, out int total, int pageIndex = 0, int pageSize = 50);

        bool TimeEntryExists(ServerTimeEntry entity);
    }
}