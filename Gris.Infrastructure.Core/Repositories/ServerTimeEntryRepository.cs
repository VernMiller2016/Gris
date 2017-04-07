using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System.Data.Entity;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerTimeEntryRepository : EFRepository<ServerTimeEntry>, IServerTimeEntryRepository
    {
        public ServerTimeEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public bool TimeEntryExists(ServerTimeEntry entity)
        {
            return this.Contains(t => t.ServerId == entity.ServerId && t.PaySourceId == entity.PaySourceId
            && t.Duration == entity.Duration && DbFunctions.TruncateTime(t.BeginDate) == DbFunctions.TruncateTime(entity.BeginDate));
        }
    }
}