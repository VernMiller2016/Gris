using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Infrastructure.Core.Interfaces
{
   public interface IServerSalaryReportRepository : IRepository<ServerSalaryReportEntity>
    {
      IEnumerable<ServerSalaryReportEntity>  GetServerSalaryDataByMonth(DateTime date);
    }
}
