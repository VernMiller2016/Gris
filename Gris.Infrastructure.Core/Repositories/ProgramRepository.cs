using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ProgramRepository : SoftDeleteEFRepository<Program>, IProgramRepository
    {
        public ProgramRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}