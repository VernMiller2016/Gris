using Gris.Application.Core.Contracts.Reports;
using Gris.Application.Core.Enums;
using Gris.Application.Core.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Gris.Application.Core.Services
{
    public class ExcelExportingService : IExportingService
    {
        private IServerTimeEntryService _serverTimeEntryService;

        public ExcelExportingService(IServerTimeEntryService serverTimeEntryService)
        {
            _serverTimeEntryService = serverTimeEntryService;
        }

        public MemoryStream GetServerTimeEntriesMonthlyReportExcel(DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.ServerTimeEntriesMonthly);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateServerTimeEntriesMonthlyReportExcel(package, _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(time), time);

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }

        #region ServerTimeEntriesMonthlyReport

        private void GenerateServerTimeEntriesMonthlyReportExcel(ExcelPackage excelPackage, IEnumerable<ServerTimeEntriesMonthlyReportEntity> reportData, DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1]; // main sheet that contains all records.

            var index = 3;
            foreach (var item in reportData)
            {
                dataSheet.Cells["A" + index].Value = item.ServerVendorId;
                dataSheet.Cells["B" + index].Value = item.ServerName;
                dataSheet.Cells["C" + index].Value = item.BeginDate.ToString(Constants.ShortDateFormat);
                dataSheet.Cells["D" + index].Value = item.Duration.ToString();
                dataSheet.Cells["E" + index].Value = item.PaysourceVendorId;
                index++;
            }
            dataSheet.Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(time.Month) + " - " + time.Year;
            dataSheet.Cells.AutoFitColumns();
        }

        #endregion ServerTimeEntriesMonthlyReport

        #region Private Methods

        private string GetExcelTemplate(ReportType type)
        {
            //Open Template and copy a new file from the template
            string templatePath = String.Empty;

            switch (type)
            {
                case ReportType.ServerTimeEntriesMonthly:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\ServerTimeEntriesMonthlyReportTemplate.xlsx";
                    break;

                default:
                    templatePath = String.Empty;
                    break;
            }

            return templatePath;
        }

        #endregion Private Methods
    }
}