﻿using Gris.Application.Core.Contracts.Paging;
using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IServerService
    {
        IEnumerable<Server> GetServers(PagingInfo pagingInfo = null);

        Server GetById(int id);

        Server GetByVendorId(int vendorId);

        void AddServer(Server server);

        void UpdateServer(Server server);

        void AddServers(IEnumerable<Server> servers);

        void Remove(Server server);
    }
}