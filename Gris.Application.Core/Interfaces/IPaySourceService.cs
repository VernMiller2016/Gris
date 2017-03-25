using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IPaySourceService
    {
        IEnumerable<PaySource> GetPaySources();        

        PaySource GetById(int id);

        PaySource GetByVendorId(int vendorId);

        void AddPaySource(PaySource paySource);

        void UpdatePaySource(PaySource paySource);

        IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources);

        void Remove(PaySource paySource);
    }
}