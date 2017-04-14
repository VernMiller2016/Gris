using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ServerAvailableHourService : IServerAvailableHourService
    {
        private IRepository<ServerAvailableHour> _serverAvailableHourRepoitory;
        private IUnitOfWork _unitOfWork;

        public ServerAvailableHourService(IRepository<ServerAvailableHour> serverAvailableHourRepoitory, IUnitOfWork unitOfWork)
        {
            _serverAvailableHourRepoitory = serverAvailableHourRepoitory;
            _unitOfWork = unitOfWork;
        }

        public void Add(IEnumerable<ServerAvailableHour> entities)
        {
            _serverAvailableHourRepoitory.Add(entities);
            _unitOfWork.Commit();
        }

        public void Add(ServerAvailableHour entity)
        {
            _serverAvailableHourRepoitory.Add(entity);
            _unitOfWork.Commit();
        }

        public IEnumerable<ServerAvailableHour> GetByServerId(int serverId)
        {
            return _serverAvailableHourRepoitory.Get(t => t.ServerId == serverId
            , (list => list.OrderBy(t => t.Server.FullName))
            , t => t.Server);
        }

        public IEnumerable<ServerAvailableHour> GetByServerVendorId(int serverVendorId)
        {
            return _serverAvailableHourRepoitory.Get(t => t.Server.VendorId == serverVendorId
            , (list => list.OrderBy(t => t.Server.FullName))
            , t => t.Server);
        }

        public IEnumerable<ServerAvailableHour> GetByDate(DateTime selectedDate, PagingInfo pagingInfo = null)
        {
            if (pagingInfo == null)
            {
                return _serverAvailableHourRepoitory.Get(t => t.DateRange.Year == selectedDate.Year && t.DateRange.Month == selectedDate.Month
                    , (list => list.OrderBy(t => t.Server.FullName))
                    , t => t.Server);
            }
            else
            {
                int total = 0;
                var result = _serverAvailableHourRepoitory.FilterWithPaging(t => t.DateRange.Year == selectedDate.Year && t.DateRange.Month == selectedDate.Month
                , (list => list.OrderBy(t => t.Server.FullName))
                , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server);
                pagingInfo.Total = total;
                return result;
            }
        }

        public void Update(ServerAvailableHour entity)
        {
            _serverAvailableHourRepoitory.Update(entity);
            _unitOfWork.Commit();
        }
    }
}