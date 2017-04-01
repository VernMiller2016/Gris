using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;

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

        public void AddPaySource(PaySource paySource)
        {
            _paySourceRepoitory.Add(paySource);
            _unitOfWork.Commit();
        }

        public void AddPaySources(IEnumerable<PaySource> paySources)
        {
            _paySourceRepoitory.Add(paySources);
            _unitOfWork.Commit();
        }

        public PaySource GetById(int id)
        {
            return _paySourceRepoitory.GetById(id);
        }

        public PaySource GetByVendorId(int vendorId)
        {
            return _paySourceRepoitory.OneOrDefault(t => t.VendorId== vendorId);
        }

        public IEnumerable<PaySource> GetPaySources()
        {
            return _paySourceRepoitory.GetAll();
        }

        public void Remove(PaySource paySource)
        {
            _paySourceRepoitory.Delete(paySource);
            _unitOfWork.Commit();
        }

        public void UpdatePaySource(PaySource paySource)
        {
            _paySourceRepoitory.Update(paySource);
            _unitOfWork.Commit();
        }
    }
}