using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerService
    {
        IEnumerable<Server> GetServers();

        Server GetById(int id);

        Server GetByServerId(int serverId);

        void AddServer(Server server);

        void UpdateServer(Server server);

        IEnumerable<Server> AddServers(IEnumerable<Server> servers);

        void Remove(Server server);
    }
}