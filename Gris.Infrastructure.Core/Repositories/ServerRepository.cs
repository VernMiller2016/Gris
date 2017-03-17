using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRis.Models;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerRepository : BaseRepository, IServerRepository
    {
        public ServerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Server AddServer(Server server)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Server> AddServers(IEnumerable<Server> servers)
        {
            throw new NotImplementedException();
        }

        public Server GetServerById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Server> GetServers()
        {
            throw new NotImplementedException();
        }

        public Server UpdateServer(Server server)
        {
            throw new NotImplementedException();
        }
    }
}
