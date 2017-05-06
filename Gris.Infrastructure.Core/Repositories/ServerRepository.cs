using System;
using System.Collections.Generic;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System.Linq;

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

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }
    }
}