using System;
using System.IO;

namespace Gris.Application.Core.Interfaces
{
    public interface IExportingService
    {
        MemoryStream GetServerTimeEntriesMonthlyReportExcel(DateTime time);

        MemoryStream GetServerAvailableHoursTemplate(DateTime time);

        MemoryStream GetStaffPercentagesMonthlyReportExcel(DateTime time);
    }
}