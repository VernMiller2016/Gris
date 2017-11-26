using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
            return _paySourceRepoitory.OneOrDefault(t => t.Id == id, t => t.Programs);
        }

        public PaySource GetByVendorId(int vendorId)
        {
            return _paySourceRepoitory.OneOrDefault(t => t.VendorId == vendorId, t => t.Programs);
        }

        public IEnumerable<PaySource> GetPaySources(PagingInfo pagingInfo = null, string search = "")
        {
            if (pagingInfo == null)
                return _paySourceRepoitory.Get(null, (list => list.OrderBy(p => p.Description)), t => t.Programs);
            else
            {
                int total = 0;
                IEnumerable<PaySource> result = null;
                if (!string.IsNullOrEmpty(search))
                {
                    result = _paySourceRepoitory.FilterWithPaging(s => s.Description.ToLower().Contains(search.ToLower()), (list => list.OrderBy(p => p.Description)), out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Programs);
                }
                else
                {
                    result = _paySourceRepoitory.FilterWithPaging(null, (list => list.OrderBy(p => p.Description)), out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Programs);
                }
                pagingInfo.Total = total;
                return result;
            }
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