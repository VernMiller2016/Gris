using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;

namespace Gris.Infrastructure.Core.Repositories
{
    public class PaySourceRepository : BaseRepository , IPaySourceRepository
    {
        public PaySourceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public PaySource AddPaySource(PaySource paySource)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources)
        {
            var addedpaySources = _dbContext.PaySources.AddRange(paySources);
            return addedpaySources;
        }

        public PaySource GetServerById(int id)
        {
            var paySource = _dbContext.PaySources.SingleOrDefault(s => s.PaySourceId == id);
            return paySource;
        }

        public IEnumerable<PaySource> GetServers()
        {
            return _dbContext.PaySources.ToList();
        }

        public void Remove(PaySource paySource)
        {
            throw new NotImplementedException();
        }

        public PaySource UpdateServer(PaySource paySource)
        {
            var oldpaySource = _dbContext.PaySources.SingleOrDefault(p => p.PaySourceId == paySource.PaySourceId);
            oldpaySource.Active = paySource.Active;
            oldpaySource.Description = paySource.Description;
            oldpaySource.PaySourceId = paySource.PaySourceId;
            oldpaySource.ProgramId = paySource.ProgramId;
            _dbContext.Entry(oldpaySource).State = System.Data.Entity.EntityState.Modified;
            return oldpaySource;
        }
    }
}
