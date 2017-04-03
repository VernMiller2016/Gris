﻿using AutoMapper;
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
        private IRepository<ServerTimeEntry> _serverTimeEntryRepoitory;
        private IUnitOfWork _unitOfWork;

        public ServerTimeEntryService(IRepository<ServerTimeEntry> serverTimeEntryRepoitory, IUnitOfWork unitOfWork)
        {
            _serverTimeEntryRepoitory = serverTimeEntryRepoitory;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ServerTimeEntry> AddServerTimeEntries(IEnumerable<ServerTimeEntry> entities)
        {
            _serverTimeEntryRepoitory.Add(entities);
            _unitOfWork.Commit();
            return entities;
        }

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries(PagingInfo pagingInfo = null)
        {
            if (pagingInfo == null)
            {
                return _serverTimeEntryRepoitory.Get(null, (list => list.OrderBy(t => t.Server.LastName))
                    , t => t.Server, t => t.PaySource);
            }
            else
            {
                int total = 0;
                var result = _serverTimeEntryRepoitory.FilterWithPaging(null, (list => list.OrderBy(t => t.Server.LastName))
                    , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.Server, t => t.PaySource);
                pagingInfo.Total = total;
                return result;
            }
        }

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries(DateTime selectedDate, PagingInfo pagingInfo)
        {
            if (pagingInfo == null)
            {
                return _serverTimeEntryRepoitory.Get(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                , (list => list.OrderBy(st => st.Server.LastName)), t => t.Server, t => t.PaySource);
            }
            else
            {
                int total = 0;
                var result = _serverTimeEntryRepoitory.FilterWithPaging(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                , (list => list.OrderBy(st => st.Server.LastName))
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
                            , (list => list.OrderBy(st => st.Server.LastName))
                            , st => st.PaySource, st => st.PaySource.Program, st => st.Server)
                            .Where(st => st.PaySource != null && st.PaySource.ProgramId.HasValue);
            }
            else
            {
                int total = 0;
                result = _serverTimeEntryRepoitory.
                            FilterWithPaging(t => t.BeginDate.Year == selectedDate.Year && t.BeginDate.Month == selectedDate.Month
                            , (list => list.OrderBy(st => st.Server.LastName))
                            , out total, pagingInfo.PageIndex, AppSettings.PageSize
                            , st => st.PaySource, st => st.PaySource.Program, st => st.Server)
                            .Where(st => st.PaySource != null && st.PaySource.ProgramId.HasValue);
                pagingInfo.Total = total;
            }
            return Mapper.Map<IEnumerable<ServerTimeEntriesMonthlyReportEntity>>(result);
        }
    }
}