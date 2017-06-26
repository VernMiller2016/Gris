using Gris.Application.Core.Contracts.Paging;
using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IElementService
    {
        IEnumerable<Element> GetElements(PagingInfo pagingInfo = null);

        Element GetById(int id);

        Element GetByVendorId(int vendorId);

        void AddElement(Element element);

        void UpdateElement(Element element);

        void Remove(Element element);
    }
}