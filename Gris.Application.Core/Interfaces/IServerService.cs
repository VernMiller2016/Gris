using GRis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerService
    {
        IEnumerable<Server> GetServers();
        Server GetServerById(int id);
        Server AddServer(Server server);
        Server UpdateServer(Server server);
        IEnumerable<Server> AddServers(IEnumerable<Server> servers);
        void Remove(Server server);
    }
}
