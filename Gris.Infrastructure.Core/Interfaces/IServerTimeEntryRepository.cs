using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IServerTimeEntryRepository : IRepository<ServerTimeEntry>
    {
        List<ServerTimeEntry> SearchForEntries(DateTime? selectedDate, string firstName,string secondName, string paysourceName, out int total, int pageIndex = 0, int pageSize = 50);

        bool TimeEntryExists(ServerTimeEntry entity);
    }
}