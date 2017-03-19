using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IServerRepository : IRepository<Server>
    {
        IEnumerable<Server> AddServers(IEnumerable<Server> servers);
    }
}