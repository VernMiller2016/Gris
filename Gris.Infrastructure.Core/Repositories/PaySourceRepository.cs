using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Infrastructure.Core.Repositories
{
    public class PaySourceRepository : EFRepository<PaySource>, IPaySourceRepository
    {
        public PaySourceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Delete(PaySource entity)
        {
            entity.Active = false;
            this.Update(entity);
        }
    }
}