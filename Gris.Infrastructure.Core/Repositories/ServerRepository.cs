using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<Server> SearchForServers(string firstName, string lastName, out int total, int pageIndex = 0, int pageSize = 50)
        {
            var query = _dbSet.Include(t => t.Category).Include(t => t.Element);

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(t => t.FirstName.ToLower().Contains(firstName.ToLower()));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(t => t.LastName.ToLower().Contains(lastName.ToLower()));

            if (pageSize > 0)
            {
                var result = query.OrderBy(t => new { t.LastName, t.FirstName }).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                total = query.Count();
                return result;
            }
            else
            {
                var result = query.OrderBy(t => new { t.LastName, t.FirstName }).ToList();
                total = result.Count;
                return result;
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }
    }
}