using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerRepository : EFRepository<Server>, IServerRepository
    {
        public ServerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Delete(Server entity)
        {
            entity.Active = false;
            this.Update(entity);
        }
    }
}