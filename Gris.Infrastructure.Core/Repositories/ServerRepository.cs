using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerRepository : SoftDeleteEFRepository<Server>, IServerRepository
    {
        public ServerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Server> AddServers(IEnumerable<Server> servers)
        {
            var addedServers = _dbContext.Servers.AddRange(servers);
            return addedServers;
        }        
    }
}