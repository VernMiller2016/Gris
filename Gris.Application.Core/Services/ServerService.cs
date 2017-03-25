using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;

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

        public IEnumerable<Server> AddServers(IEnumerable<Server> servers)
        {
            var addedServers = _serverRepoitory.AddServers(servers);
            _unitOfWork.Commit();
            return addedServers;
        }

        public Server GetById(int id)
        {
            return _serverRepoitory.GetById(id);
        }

        public Server GetByVendorId(int vendorId)
        {
            return _serverRepoitory.OneOrDefault(t => t.VendorId == vendorId);
        }

        public IEnumerable<Server> GetServers()
        {
            return _serverRepoitory.GetAll();
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