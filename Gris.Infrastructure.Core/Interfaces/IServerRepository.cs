using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Infrastructure.Core.Interfaces
{
    public interface IServerRepository : IRepository<Server>
    {
        List<Server> SearchForServers(string firstName, string lastName, out int total, int pageIndex = 0, int pageSize = 50);

        IEnumerable<Category> GetAllCategories();
    }
}