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

        public IEnumerable<ServerTimeEntry> GetServerTimeEntries()
        {
            return _serverTimeEntryRepoitory.Get(null, (list => list.OrderBy(t => t.Server.LastName)), t => t.Server, t => t.PaySource);
        }

        public IEnumerable<ServerTimeEntriesMonthlyReportEntity> GetServerTimeEntriesMonthlyReport(DateTime time)
        {
            // ToDo: use automapper
            var result = _serverTimeEntryRepoitory.
                        Get(t => t.BeginDate.Year == time.Year && t.BeginDate.Month == time.Month
                        , (list => list.OrderBy(st => st.Server.LastName))
                        , st => st.PaySource, st => st.PaySource.Program, st => st.Server)
                        .Where(st => st.PaySource != null && st.PaySource.ProgramId.HasValue)
                        .Select(st => new ServerTimeEntriesMonthlyReportEntity()
                        {
                            ServerName = st.Server.FullName,
                            ServerVendorId = st.Server.VendorId,
                            BeginDate = st.BeginDate.Date,
                            Duration = st.Duration,
                            PaysourceVendorId = st.PaySource.VendorId,
                            ProgramId = st.PaySource.Program.Id,
                            ProgramName = st.PaySource.Program.Name
                        });
            return result;
        }
    }
}