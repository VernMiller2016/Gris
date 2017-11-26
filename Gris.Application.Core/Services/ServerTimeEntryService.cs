using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ServerTimeEntryService : IServerTimeEntryService
    {
        private IServerTimeEntryRepository _serverTimeEntryRepoitory;
        private IUnitOfWork _unitOfWork;

        public ServerTimeEntryService(IServerTimeEntryRepository serverTimeEntryRepoitory, IUnitOfWork unitOfWork)
        {
            _serverTimeEntryRepoitory = serverTimeEntryRepoitory;
            _unitOfWork = unitOfWork;
        }

        public ServerTimeEntry GetById(int id)
        {
            return _serverTimeEntryRepoitory.OneOrDefault(t => t.Id == id, t => t.Server, t => t.PaySource, t => t.Program);
        }

        public bool TimeEntryExists(ServerTimeEntry entity)
        {
            return _serverTimeEntryRepoitory.TimeEntryExists(entity);
        }

        public void AddServerTimeEntry(ServerTimeEntry entity)
        {
            _serverTimeEntryRepoitory.Add(entity);
            _unitOfWork.Commit();
        }

        public void AddServerTimeEntries(IEnumerable<ServerTimeEntry> entities)
        {
            _serverTimeEntryRepoitory.Add(entities);
            _unitOfWork.Commit();
        }

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries(DateTime? selectedDate, string serverName, string paysourceName, PagingInfo pagingInfo = null)
        {
            int total = 0;
            IEnumerable<ServerTimeEntry> result = null;
            if (pagingInfo == null)
            {
                result = _serverTimeEntryRepoitory.SearchForEntries(selectedDate, serverName, paysourceName, out total, -1, -1);
            }
            else
            {
                result = _serverTimeEntryRepoitory.SearchForEntries(selectedDate, serverName, paysourceName, out total, pagingInfo.PageIndex, AppSettings.PageSize);
                pagingInfo.Total = total;
            }
            return result;
        }

        public IEnumerable<ServerTimeEntriesMonthlyReportEntity> GetServerTimeEntriesMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null)
        {
            var result = Enumerable.Empty<ServerTimeEntry>();
            if (pagingInfo == null)
            {
                result = _serverTimeEntryRepoitory.
                            Get(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                            && t.PaySource != null && t.ProgramId.HasValue
                            , (list => list.OrderByDescending(st => st.BeginDate))
                            , st => st.PaySource, st => st.Program, st => st.Server, st => st.Server.Category)
                            ;
            }
            else
            {
                int total = 0;
                result = _serverTimeEntryRepoitory.
                            FilterWithPaging(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                            && t.PaySource != null && t.ProgramId.HasValue
                            , (list => list.OrderByDescending(st => st.BeginDate))
                            , out total, pagingInfo.PageIndex, AppSettings.PageSize
                            , st => st.PaySource, st => st.Program, st => st.Server)
                            ;
                pagingInfo.Total = total;
            }
            return Mapper.Map<IEnumerable<ServerTimeEntriesMonthlyReportEntity>>(result);
        }

        public void UpdateServerTimeEntry(ServerTimeEntry entity, bool applyProgramEditToAllEntries = false)
        {
            _serverTimeEntryRepoitory.Update(entity);
            if (applyProgramEditToAllEntries)
            {
                _serverTimeEntryRepoitory.BatchUpdate(t => (t.ServerId == entity.ServerId && t.PaySourceId == entity.PaySourceId)
                , t => new ServerTimeEntry() { ProgramId = entity.ProgramId });
            }
            _unitOfWork.Commit();
        }

        public void Remove(ServerTimeEntry entity)
        {
            _serverTimeEntryRepoitory.Delete(entity);
            _unitOfWork.Commit();
        }
    }
}