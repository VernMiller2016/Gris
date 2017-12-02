using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Application.Core.Interfaces
{
   public interface IServerSalaryReportService
    {
        IEnumerable<ServerSalaryReportViewModel> GetServerSalaryMonthlyReport(DateTime selectedDate, PagingInfo pagingInfo = null);
        IEnumerable<ServerSalaryReportViewModel> GetServerSalaryMonthlyPercentageReport(DateTime selectedDate, PagingInfo pagingInfo = null);
    }
}
