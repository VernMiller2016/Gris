using Gris.Infrastructure.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Infrastructure.Core.Repositories
{
    public class BaseRepository : IDisposable
    {
        protected ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
