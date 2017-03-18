using Gris.Application.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gris.Infrastructure.Core.Interfaces;
using Gris.Domain.Core.Models;

namespace Gris.Application.Core.Services
{
    public class ServerService : IServerService
    {
        private IServerRepository _serverRepoitory;
        private IUnitOfWork _unitOfWork;
        public ServerService(IServerRepository serverRepository,IUnitOfWork unitOfWork)
        {
            _serverRepoitory = serverRepository;
            _unitOfWork = unitOfWork;
        }
        public Server AddServer(Server server)
        {
          return  _serverRepoitory.AddServer(server);
        }

        public IEnumerable<Server> AddServers(IEnumerable<Server> servers)
        {
            var addedServers = _serverRepoitory.AddServers(servers);
            _unitOfWork.Commit();
            return addedServers;
        }

        public Server GetServerById(int id)
        {
            return _serverRepoitory.GetServerById(id);
        }

        public IEnumerable<Server> GetServers()
        {
            return _serverRepoitory.GetServers();
        }

        public void Remove(Server server)
        {
            _serverRepoitory.Remove(server);
        }

        public Server UpdateServer(Server server)
        {
            var updatedServer = _serverRepoitory.UpdateServer(server);
            _unitOfWork.Commit();
            return updatedServer;
        }
    }
}
