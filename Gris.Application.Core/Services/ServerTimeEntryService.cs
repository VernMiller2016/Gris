﻿using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;
using System;
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
    }
}