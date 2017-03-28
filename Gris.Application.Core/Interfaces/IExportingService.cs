using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Application.Core.Interfaces
{
    public interface IExportingService
    {
        MemoryStream GetServerTimeEntriesMonthlyReportExcel(DateTime time);
    }
}
