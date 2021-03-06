﻿using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ServerService : IServerService
    {
        private IServerRepository _serverRepoitory;
        private IUnitOfWork _unitOfWork;

        public ServerService(IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            _serverRepoitory = serverRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddServer(Server server)
        {
            _serverRepoitory.Add(server);
            _unitOfWork.Commit();
        }

        public void AddServers(IEnumerable<Server> servers)
        {
            _serverRepoitory.Add(servers);
            _unitOfWork.Commit();
        }

        public Server GetById(int id)
        {
            return _serverRepoitory.OneOrDefault(t => t.Id == id, t => t.Element);
        }

        public Server GetByVendorId(int vendorId)
        {
            return _serverRepoitory.OneOrDefault(t => t.VendorId == vendorId);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _serverRepoitory.GetAllCategories().OrderBy(t => t.Name);
        }

        public IEnumerable<Server> GetServers(PagingInfo pagingInfo = null, string firstName = "", string lastName = "")
        {
            int total = 0;
            IEnumerable<Server> result = null;

            if (pagingInfo == null)
                return _serverRepoitory.SearchForServers(firstName, lastName, out total, -1, -1);
            else
            {
                result = _serverRepoitory.SearchForServers(firstName, lastName, out total, pagingInfo.PageIndex, AppSettings.PageSize);
                pagingInfo.Total = total;
            }
            return result;
        }

        public void Remove(Server server)
        {
            _serverRepoitory.Delete(server);
            _unitOfWork.Commit();
        }

        public void UpdateServer(Server server)
        {
            _serverRepoitory.Update(server);
            _unitOfWork.Commit();
        }
    }
}