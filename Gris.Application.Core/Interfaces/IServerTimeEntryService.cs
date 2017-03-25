using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerTimeEntryService
    {
        IEnumerable<ServerTimeEntry> GetServerTimeEntries();

        IEnumerable<ServerTimeEntry> GetServerTimeEntriesByMonthAndYear(DateTime time);

        IEnumerable<ServerTimeEntry> AddServerTimeEntries(IEnumerable<ServerTimeEntry> entities);
    }
}