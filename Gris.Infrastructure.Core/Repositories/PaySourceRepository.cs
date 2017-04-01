using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Repositories
{
    public class PaySourceRepository : SoftDeleteEFRepository<PaySource>, IPaySourceRepository
    {
        public PaySourceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources)
        {
            var addedpaySources = _dbContext.PaySources.AddRange(paySources);
            return addedpaySources;
        }
    }
}