using Gris.Application.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Application.Core.Services
{
    public class PaySourceService : IPaySourceService
    {
        private IPaySourceRepository _paySourceRepoitory;
        private IUnitOfWork _unitOfWork;

        public PaySourceService(IPaySourceRepository paySourceRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _paySourceRepoitory = paySourceRepository;
        }
        public PaySource AddPaySource(PaySource paySource)
        {
            return _paySourceRepoitory.AddPaySource(paySource);
        }

        public IEnumerable<PaySource> AddPaySources(IEnumerable<PaySource> paySources)
        {
            var addedServers = _paySourceRepoitory.AddPaySources(paySources);
            _unitOfWork.Commit();
            return addedServers;
        }

        public PaySource GetPaySourceById(int id)
        {
            return _paySourceRepoitory.GetServerById(id);
        }

        public IEnumerable<PaySource> GetPaySources()
        {
            return _paySourceRepoitory.GetServers();
        }

        public void Remove(PaySource paySource)
        {
            _paySourceRepoitory.Remove(paySource);
        }

        public PaySource UpdatePaySource(PaySource server)
        {
            var updatedPaySource = _paySourceRepoitory.UpdateServer(server);
            _unitOfWork.Commit();
            return updatedPaySource;
        }
    }
}
