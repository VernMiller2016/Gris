using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IPaySourceRepository
    {
        PaySource AddPaySource(PaySource paySource);
        IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources);
        PaySource GetServerById(int id);
        IEnumerable<PaySource> GetServers();
        void Remove(PaySource paySource);
        PaySource UpdateServer(PaySource paySource);
    }
}
