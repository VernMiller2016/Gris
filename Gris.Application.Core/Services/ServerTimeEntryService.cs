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
            return _serverTimeEntryRepoitory.OneOrDefault(t => t.Id == id, t => t.Server, t => t.PaySource);
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

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries(PagingInfo pagingInfo = null)
        {
            if (pagingInfo == null)
            {
                return _serverTimeEntryRepoitory.Get(null, (list => list.OrderBy(t => t.Server.FullName))
                    , t => t.Server, t => t.PaySource);
            }
            else
            {
                int total = 0;
                //
                IEnumerable<ServerTimeEntry> result = null;
                if (!string.IsNullOrEmpty(pagingInfo.SearchOption) && !string.IsNullOrEmpty(pagingInfo.SearchValue))
                {
                    if (pagingInfo.SearchOption == "ServerName")
                    {
                        result = _serverTimeEntryRepoitory.FilterWithPaging(s => s.Server.FirstName.ToLower().Contains(pagingInfo.SearchValue.ToLower()) || s.Server.LastName.ToLower().Contains(pagingInfo.SearchValue.ToLower()), (list => list.OrderBy(t => t.Server.FullName))
                    , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server, t => t.PaySource);
                    }
                    else if (pagingInfo.SearchOption == "PaySourceName")
                    {
                        result = _serverTimeEntryRepoitory.FilterWithPaging(s => s.PaySource.Description.ToLower().Contains(pagingInfo.SearchValue.ToLower()), (list => list.OrderBy(t => t.Server.FullName))
                    , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server, t => t.PaySource);
                    }
                }
                //
                else
                {
                    result = _serverTimeEntryRepoitory.FilterWithPaging(null, (list => list.OrderBy(t => t.Server.FullName))
                       , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server, t => t.PaySource);
                }
                pagingInfo.Total = total;
                return result;
            }
        }

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries(DateTime selectedDate, PagingInfo pagingInfo)
        {
            if (pagingInfo == null)
            {
                return _serverTimeEntryRepoitory.Get(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                , (list => list.OrderBy(st => st.Server.FullName)), t => t.Server, t => t.PaySource);
            }
            else
            {
                int total = 0;
                var result = _serverTimeEntryRepoitory.FilterWithPaging(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                , (list => list.OrderBy(st => st.Server.FullName))
                , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server, t => t.PaySource);
                pagingInfo.Total = total;
                return result;
            }
        }

        public IEnumerable<ServerTimeEntriesMonthlyReportEntity> GetServerTimeEntriesMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null)
        {
            var result = Enumerable.Empty<ServerTimeEntry>();
            if (pagingInfo == null)
            {
                result = _serverTimeEntryRepoitory.
                            Get(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                            && t.PaySource != null && t.PaySource.ProgramId.HasValue
                            , (list => list.OrderBy(st => st.Server.FullName))
                            , st => st.PaySource, st => st.PaySource.Program, st => st.Server)
                            ;
            }
            else
            {
                int total = 0;
                result = _serverTimeEntryRepoitory.
                            FilterWithPaging(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                            && t.PaySource != null && t.PaySource.ProgramId.HasValue
                            , (list => list.OrderBy(st => st.Server.FullName))
                            , out total, pagingInfo.PageIndex, AppSettings.PageSize
                            , st => st.PaySource, st => st.PaySource.Program, st => st.Server)
                            ;
                pagingInfo.Total = total;
            }
            return Mapper.Map<IEnumerable<ServerTimeEntriesMonthlyReportEntity>>(result);
        }

        public void UpdateServerTimeEntry(ServerTimeEntry entity)
        {
            _serverTimeEntryRepoitory.Update(entity);
            _unitOfWork.Commit();
        }

        public void Remove(ServerTimeEntry entity)
        {
            _serverTimeEntryRepoitory.Delete(entity);
            _unitOfWork.Commit();
        }
    }
}