using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Infrastructure.Core.Repositories
{
    public class ProgramRepository : EFRepository<Program>, IProgramRepository
    {
        public ProgramRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Delete(Program entity)
        {
            entity.Active = false;
            this.Update(entity);
        }
    }
}