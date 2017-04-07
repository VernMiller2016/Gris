using Gris.Application.Core.Contracts.Reports;
using Gris.Application.Core.Enums;
using Gris.Application.Core.Interfaces;
using GRis.Core.Extensions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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
            // 1- create sheets per program which should be copy of the first sheet.
            var index = 2; // starting index of each sheet.
            var groupByProgram = reportData.GroupBy(t => new { t.ProgramId, t.ProgramName });
            groupByProgram.ForEachWithIndex((programGroup, programIndex) =>
            {
                var programSheet = excelPackage.Workbook.Worksheets.Add(
                    programGroup.Key.ProgramName + " - " + string.Join<int>(",", programGroup.Select(t => t.PaysourceVendorId).Distinct())
                    , excelPackage.Workbook.Worksheets[1]);
                index = 2;
                var groupByServer = programGroup.GroupBy(t => new { t.ServerVendorId, t.ServerName });

                groupByServer.ForEachWithIndex((serverGroup, serverIndex) =>
                {
                    var currentServerGroupStartIndex = index;
                    foreach (var item in serverGroup)
                    {
                        programSheet.Cells["A" + index].Value = item.ServerVendorId;
                        programSheet.Cells["B" + index].Value = item.ServerName;
                        programSheet.Cells["C" + index].Value = item.BeginDate.ToString(Constants.ShortDateFormat);
                        //programSheet.Cells["D" + index].Value = DateTime.Today.Add(item.Duration).ToString("hh:mm:ss tt");
                        programSheet.Cells["D" + index].Style.Numberformat.Format = "[h]:mm:ss";
                        programSheet.Cells["D" + index].Value = item.Duration;
                        programSheet.Cells["E" + index].Value = item.PaysourceDescription;
                        programSheet.Row(index).OutlineLevel = (2);
                        programSheet.Row(index).Collapsed = true;
                        index++;
                    }
                    var currentServerGroupEndIndex = index - 1;

                    programSheet.Cells["B" + index].Value = serverGroup.Key.ServerName + " Total";
                    programSheet.Cells["B" + index].Style.Font.Bold = true;
                    programSheet.Cells["D" + index].Style.Numberformat.Format = "[h]:mm:ss";
                    programSheet.Cells["D" + index].Formula = $"=SUBTOTAL(9,D{currentServerGroupStartIndex}:D{currentServerGroupEndIndex})";
                    programSheet.Row(index).OutlineLevel = (1);
                    programSheet.Row(index).Collapsed = false;
                    index++;
                });

                programSheet.Cells["B" + index].Value = "Grand Total";
                programSheet.Cells["B" + index].Style.Font.Bold = true;
                programSheet.Cells["D" + index].Style.Numberformat.Format = "[h]:mm:ss";
                programSheet.Cells["D" + index].Formula = $"=SUBTOTAL(9,D{2}:D{index - 1})";
                //programSheet.Row(index).OutlineLevel = (1);
                //programSheet.Row(index).Collapsed = false;

                programSheet.Cells.AutoFitColumns();
                programSheet.View.TabSelected = false;
            });


            // 2- create the first sheet that contains all data.
            var dataSheet = excelPackage.Workbook.Worksheets[1]; // main sheet that contains all records.            
            index = 2;
            foreach (var item in reportData)
            {
                dataSheet.Cells["A" + index].Value = item.ServerVendorId;
                dataSheet.Cells["B" + index].Value = item.ServerName;
                dataSheet.Cells["C" + index].Value = item.BeginDate.ToString(Constants.ShortDateFormat);
                dataSheet.Cells["D" + index].Value = item.Duration.ToString();
                dataSheet.Cells["E" + index].Value = item.PaysourceDescription;
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