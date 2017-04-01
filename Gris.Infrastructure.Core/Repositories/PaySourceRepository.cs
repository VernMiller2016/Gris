using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.DAL;
using Gris.Infrastructure.Core.Interfaces;

namespace Gris.Infrastructure.Core.Repositories
{
    public class PaySourceRepository : SoftDeleteEFRepository<PaySource>, IPaySourceRepository
    {
        public PaySourceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}