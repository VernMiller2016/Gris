using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRis.Models;
using Gris.Infrastructure.Core.DAL;

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
            var addedServers = _dbContext.Servers.AddRange(servers);
            return addedServers;
        }

        public Server GetServerById(int id)
        {
            var server = _dbContext.Servers.SingleOrDefault(s => s.ServerId == id);
            return server;
        }

        public IEnumerable<Server> GetServers()
        {
           return  _dbContext.Servers.ToList();
        }

        public void Remove(Server server)
        {
            throw new NotImplementedException();
        }

        public Server UpdateServer(Server server)
        {
            var oldServer = _dbContext.Servers.SingleOrDefault(s => s.ServerId == server.ServerId);
            oldServer.Active = server.Active;
            oldServer.FirstName = server.FirstName;
            oldServer.LastName = server.LastName;
            oldServer.ServerId = server.ServerId;
            _dbContext.Entry(oldServer).State = System.Data.Entity.EntityState.Modified;
            return oldServer;
        }
    }
}
