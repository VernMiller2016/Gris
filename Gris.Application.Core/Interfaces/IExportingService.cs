using System;
using System.IO;

namespace Gris.Application.Core.Interfaces
{
    public interface IExportingService
    {
        MemoryStream GetServerTimeEntriesMonthlyReportExcel(DateTime time);
        MemoryStream GetServerSalariesMothlyReportExcel(DateTime time);

        MemoryStream GetServerAvailableHoursTemplate(int defaultAvailableHours, DateTime time);

        MemoryStream GetStaffPercentagesMonthlyReportExcel(DateTime time);

        MemoryStream GetCategoryPercentagesMonthlyReportExcel(DateTime time);
    }
}