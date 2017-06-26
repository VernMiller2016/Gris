using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ElementService : IElementService
    {
        private readonly EFRepository<Element> _elementRepoitory;
        private readonly IUnitOfWork _unitOfWork;

        public ElementService(EFRepository<Element> elementRepoitory, IUnitOfWork unitOfWork)
        {
            _elementRepoitory = elementRepoitory;
            _unitOfWork = unitOfWork;
        }

        public void AddElement(Element element)
        {
            _elementRepoitory.Add(element);
            _unitOfWork.Commit();
        }

        public Element GetById(int id)
        {
            return _elementRepoitory.GetById(id);
        }

        public Element GetByVendorId(int vendorId)
        {
            return _elementRepoitory.OneOrDefault(t => t.VendorId == vendorId);
        }

        public IEnumerable<Element> GetElements(PagingInfo pagingInfo = null)
        {
            if (pagingInfo == null)
                return _elementRepoitory.Get(null, list => list.OrderBy(t => t.DisplayName));
            else
            {
                int total = 0;
                var result = _elementRepoitory.FilterWithPaging(null, (list => list.OrderBy(p => p.DisplayName))
                    , out total, pagingInfo.PageIndex, AppSettings.PageSize);
                pagingInfo.Total = total;
                return result;
            }
        }

        public void Remove(Element element)
        {
            _elementRepoitory.Delete(element);
            _unitOfWork.Commit();
        }

        public void UpdateElement(Element element)
        {
            _elementRepoitory.Update(element);
            _unitOfWork.Commit();
        }
    }
}