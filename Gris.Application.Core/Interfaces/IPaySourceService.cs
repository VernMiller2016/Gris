using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Application.Core.Interfaces
{
    public interface IPaySourceService
    {
        IEnumerable<PaySource> GetPaySources();
        PaySource GetPaySourceById(int id);
        PaySource AddPaySource(PaySource paySource);
        PaySource UpdatePaySource(PaySource paySource);
        IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources);
        void Remove(PaySource paySource);
    }
}
