using Gris.Application.Core.Contracts.Paging;
using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerAvailableHourService
    {
        void Add(ServerAvailableHour entity);

        void Add(IEnumerable<ServerAvailableHour> entities);

        void Update(ServerAvailableHour entity);

        IEnumerable<ServerAvailableHour> GetByServerId(int serverId);

        IEnumerable<ServerAvailableHour> GetByServerVendorId(int serverVendorId);

        IEnumerable<ServerAvailableHour> GetByDate(DateTime selectedDate, PagingInfo pagingInfo = null);
    }
}