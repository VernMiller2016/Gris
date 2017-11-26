using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ServerTimeEntryRepository : EFRepository<ServerTimeEntry>, IServerTimeEntryRepository
    {
        public ServerTimeEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<ServerTimeEntry> SearchForEntries(DateTime? selectedDate, string serverName, string paysourceName, out int total, int pageIndex = 0, int pageSize = 50)
        {
            var query = _dbSet.Include(t => t.Server).Include(t => t.PaySource).Include(t => t.Program);

            if (selectedDate.HasValue)
                query = query.Where(t => t.BeginDate.Year == selectedDate.Value.Year && t.BeginDate.Month == selectedDate.Value.Month);

            if (!string.IsNullOrWhiteSpace(serverName))
                query = query.Where(t => t.Server.FirstName.ToLower().Contains(serverName.ToLower()) || t.Server.LastName.ToLower().Contains(serverName.ToLower()));

            if (!string.IsNullOrWhiteSpace(paysourceName))
                query = query.Where(t => t.PaySource.Description.ToLower().Contains(paysourceName.ToLower()));

            if (pageSize > 0)
            {
                var result = query.OrderByDescending(t => t.BeginDate).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                total = query.Count();
                return result;
            }
            else
            {
                var result = query.OrderByDescending(t => t.BeginDate).ToList();
                total = result.Count;
                return result;
            }
        }

        public bool TimeEntryExists(ServerTimeEntry entity)
        {
            return this.Contains(t => t.ServerId == entity.ServerId && t.PaySourceId == entity.PaySourceId
            && t.Duration == entity.Duration && DbFunctions.TruncateTime(t.BeginDate) == DbFunctions.TruncateTime(entity.BeginDate));
        }
    }
}