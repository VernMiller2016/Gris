using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IPaySourceRepository : IRepository<PaySource>
    {
        IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources);
    }
}