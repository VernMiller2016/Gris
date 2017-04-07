using Gris.Domain.Core.Models;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IServerTimeEntryRepository : IRepository<ServerTimeEntry>
    {
        bool TimeEntryExists(ServerTimeEntry entity);
    }
}