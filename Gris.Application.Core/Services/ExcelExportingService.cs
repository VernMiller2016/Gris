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
        private IServerSalaryReportService _serverSalariesService;
        private IServerAvailableHourService _serverAvailableHourService;
        private IServerService _serverService;

        public ExcelExportingService(IServerTimeEntryService serverTimeEntryService, IServerAvailableHourService serverAvailableHourService, IServerService serverService, IServerSalaryReportService serverSalaiesService)
        {
            _serverTimeEntryService = serverTimeEntryService;
            _serverAvailableHourService = serverAvailableHourService;
            _serverService = serverService;
            _serverSalariesService = serverSalaiesService;
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
        public MemoryStream GetServerSalariesMothlyReportExcel(DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.ServerSalariesMonthly);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateServerSalariesMonthlyReportExcel(package, _serverSalariesService.GetServerSalaryMonthlyReport(time), time);

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }


        public MemoryStream GetServerAvailableHoursTemplate(int defaultAvailableHours, DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.ServerAvailableHoursTemplate);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateServerAvailableHoursTemplate(package, _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(time), defaultAvailableHours, time);

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }

        public MemoryStream GetStaffPercentagesMonthlyReportExcel(DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.StaffPercentagesMonthly);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateStaffPercentagesMonthlyReportExcel(package, _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(time), time);

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }

        public MemoryStream GetCategoryPercentagesMonthlyReportExcel(DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.CategoryPercentagesMonthly);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateCategoryPercentagesMonthlyReportExcel(package, _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(time), time);

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

        #region ServerAvailableHoursTemplate

        private void GenerateServerAvailableHoursTemplate(ExcelPackage excelPackage, IEnumerable<ServerTimeEntriesMonthlyReportEntity> reportData, int defaultAvailableHours, DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1];
            var index = 2; // starting index.

            var availableHours = _serverAvailableHourService.GetByDate(time);
            var groupByServer = reportData.GroupBy(t => new { t.ServerVendorId, t.ServerName, t.ServerId });
            groupByServer.ForEachWithIndex((serverGroup, serverIndex) =>
            {
                dataSheet.Cells["A" + index].Value = serverGroup.Key.ServerVendorId;
                dataSheet.Cells["B" + index].Value = serverGroup.Key.ServerName;
                dataSheet.Cells["C" + index].Value = time.ToMonthYear();
                var existedAvailableHour = availableHours.FirstOrDefault(t => t.ServerId == serverGroup.Key.ServerId);
                if (existedAvailableHour != null)
                    dataSheet.Cells["D" + index].Value = existedAvailableHour.AvailableHours;
                else
                    dataSheet.Cells["D" + index].Value = defaultAvailableHours;
                index++;
            });

            dataSheet.Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(time.Month) + " - " + time.Year;
            dataSheet.Cells.AutoFitColumns();
        }

        #endregion ServerAvailableHoursTemplate

        #region StaffPercentagesMonthlyReport

        private void GenerateStaffPercentagesMonthlyReportExcel(ExcelPackage excelPackage, IEnumerable<ServerTimeEntriesMonthlyReportEntity> reportData, DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1];
            var rowIndex = 1; // starting row index.
            var programColIndex = 2; // starting column index of programs.
            var availableHours = _serverAvailableHourService.GetByDate(time);

            #region Setup headers

            // first cell which indicate selected date
            dataSheet.Cells[1, 1].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(time.Month) + " - " + time.Year;
            // programs as column header
            var programs = reportData.Select(t => new { t.ProgramId, t.ProgramName }).Distinct().OrderBy(t => t.ProgramName).ToList();
            if (programs.Any())
            {
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                    dataSheet.Cells[rowIndex, programColIndex].Value = program.ProgramName;
                    programColIndex++;
                });
                // "Total" column.
                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "Total";
                programColIndex++;
                // programs as percentage columns headers
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                    dataSheet.Cells[rowIndex, programColIndex].Value = $"% {program.ProgramName}";
                    programColIndex++;
                });
                // "Total"(s) column(s).
                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "Total";
                programColIndex++;

                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "% Direct Service";
                programColIndex++;

                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "Hours Available";
            }
            #endregion Setup headers

            rowIndex++;

            var groupByServer = reportData.GroupBy(t => new { t.ServerVendorId, t.ServerName, t.ServerId });
            groupByServer.ForEachWithIndex((serverGroup, serverIndex) =>
            {
                var currentColIndex = 1;
                dataSheet.Cells[rowIndex, currentColIndex].Value = serverGroup.Key.ServerName;
                dataSheet.Cells[rowIndex, currentColIndex].Style.Font.Bold = true;
                currentColIndex++;
                var startIndexOfServerProgram = currentColIndex;
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "[h]:mm:ss";
                    dataSheet.Cells[rowIndex, currentColIndex].Value = serverGroup.Where(t => t.ProgramId == program.ProgramId)
                    .Sum(t => t.Duration);
                    currentColIndex++;
                });

                var indexOfServerProgramTotal = currentColIndex;
                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "[h]:mm:ss";
                dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=SUM(${dataSheet.Cells[rowIndex, startIndexOfServerProgram].Address}"
                        + $":${dataSheet.Cells[rowIndex, startIndexOfServerProgram + (programs.Count - 1)].Address})";
                currentColIndex++;

                var startIndexOfServerProgramPercentage = currentColIndex;
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "0.00 %";
                    dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=IF(${dataSheet.Cells[rowIndex, indexOfServerProgramTotal].Address}=0,0,"
                    + $"+${dataSheet.Cells[rowIndex, startIndexOfServerProgram + programIndex].Address}/${dataSheet.Cells[rowIndex, indexOfServerProgramTotal].Address})";
                    currentColIndex++;
                });

                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "0.00 %";
                dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=SUM(${dataSheet.Cells[rowIndex, startIndexOfServerProgramPercentage].Address}"
                        + $":${dataSheet.Cells[rowIndex, startIndexOfServerProgramPercentage + (programs.Count - 1)].Address})";
                currentColIndex++;

                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "0.00 %";
                dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=IF(${dataSheet.Cells[rowIndex, (currentColIndex + 1)].Address}=0,0,"
                    + $"+${dataSheet.Cells[rowIndex, indexOfServerProgramTotal].Address}/${dataSheet.Cells[rowIndex, (currentColIndex + 1)].Address})";
                currentColIndex++;

                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "[h]:mm:ss";
                dataSheet.Cells[rowIndex, currentColIndex].Value = TimeSpan.FromHours(!availableHours.AsNotNull().Any() ? 0 : availableHours.FirstOrDefault(t => t.ServerId == serverGroup.Key.ServerId).AvailableHours);
                rowIndex++;
            });

            dataSheet.Cells.AutoFitColumns();
        }

        #endregion StaffPercentagesMonthlyReport

        #region CategoryPercentagesMonthlyReport

        private void GenerateCategoryPercentagesMonthlyReportExcel(ExcelPackage excelPackage, IEnumerable<ServerTimeEntriesMonthlyReportEntity> reportData, DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1];
            var rowIndex = 1; // starting row index.
            var programColIndex = 2; // starting column index of programs.
            var availableHours = _serverAvailableHourService.GetByDate(time);
            var categories = _serverService.GetCategories();

            #region Setup headers

            // first cell which indicate selected date
            dataSheet.Cells[1, 1].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(time.Month) + " - " + time.Year;
            // programs as column header
            var programs = reportData.Select(t => new { t.ProgramId, t.ProgramName }).Distinct().OrderBy(t => t.ProgramName).ToList();
            if (programs.Any())
            {
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                    dataSheet.Cells[rowIndex, programColIndex].Value = program.ProgramName;
                    programColIndex++;
                });
                // "Total" column.
                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "Total";
                programColIndex++;
                // programs as percentage columns headers
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                    dataSheet.Cells[rowIndex, programColIndex].Value = $"% {program.ProgramName}";
                    programColIndex++;
                });
                // "Total"(s) column(s).
                dataSheet.Cells[1, 1].Copy(dataSheet.Cells[rowIndex, programColIndex]);
                dataSheet.Cells[rowIndex, programColIndex].Value = "Total";
                programColIndex++;
            }
            #endregion Setup headers

            rowIndex++;

            foreach (var category in categories)
            {
                var currentColIndex = 1;
                dataSheet.Cells[rowIndex, currentColIndex].Value = category.Name;
                dataSheet.Cells[rowIndex, currentColIndex].Style.Font.Bold = true;
                currentColIndex++;

                var categoryGroup = reportData.Where(t => t.ServerCategoryId == category.Id);
                //groupByCategory.ForEachWithIndex((categoryGroup, categoryIndex) =>
                //{                    
                var startIndexOfCategoryProgram = currentColIndex;
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "[h]:mm:ss";
                    dataSheet.Cells[rowIndex, currentColIndex].Value = categoryGroup.Where(t => t.ProgramId == program.ProgramId)
                    .Sum(t => t.Duration);
                    currentColIndex++;
                });

                var indexOfCategoryProgramTotal = currentColIndex;
                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "[h]:mm:ss";
                dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=SUM(${dataSheet.Cells[rowIndex, startIndexOfCategoryProgram].Address}"
                        + $":${dataSheet.Cells[rowIndex, startIndexOfCategoryProgram + (programs.Count - 1)].Address})";
                currentColIndex++;

                var startIndexOfCategoryProgramPercentage = currentColIndex;
                programs.ForEachWithIndex((program, programIndex) =>
                {
                    dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "0.00 %";
                    dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=IF(${dataSheet.Cells[rowIndex, indexOfCategoryProgramTotal].Address}=0,0,"
                    + $"+${dataSheet.Cells[rowIndex, startIndexOfCategoryProgram + programIndex].Address}/${dataSheet.Cells[rowIndex, indexOfCategoryProgramTotal].Address})";
                    currentColIndex++;
                });

                dataSheet.Cells[rowIndex, currentColIndex].Style.Numberformat.Format = "0.00 %";
                dataSheet.Cells[rowIndex, currentColIndex].Formula = $"=SUM(${dataSheet.Cells[rowIndex, startIndexOfCategoryProgramPercentage].Address}"
                        + $":${dataSheet.Cells[rowIndex, startIndexOfCategoryProgramPercentage + (programs.Count - 1)].Address})";
                currentColIndex++;

                rowIndex++;
                //});
            }
            dataSheet.Cells.AutoFitColumns();
        }

        #endregion StaffPercentagesMonthlyReport

        #region ServerMonthlyReport

        private void GenerateServerSalariesMonthlyReportExcel(ExcelPackage excelPackage, IEnumerable<ServerSalaryReportViewModel> reportData, DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1];
            var index = 2; // starting index of each sheet.
            foreach (var item in reportData)
            {
                dataSheet.Cells["A" + index].Value = item.ServerName;
                dataSheet.Cells["B" + index].Value = item.SalaryAccount != null ? item.SalaryAccount.Value : 0;
                dataSheet.Cells["C" + index].Value = item.TempHelpAccount != null ? item.TempHelpAccount.Value : 0;
                dataSheet.Cells["D" + index].Value = item.OverTimeAccount != null ? item.OverTimeAccount.Value : 0;
                dataSheet.Cells["E" + index].Value = item.RetirementAccount != null ? item.RetirementAccount.Value : 0;
                dataSheet.Cells["F" + index].Value = item.SocialSecurityAccount != null ? item.SocialSecurityAccount.Value : 0;
                dataSheet.Cells["G" + index].Value = item.MedicalAndLifeInsuranceAccount != null ? item.MedicalAndLifeInsuranceAccount.Value : 0;
                dataSheet.Cells["H" + index].Value = item.IndustrialInsuranceAccount != null ? item.IndustrialInsuranceAccount.Value : 0;
                dataSheet.Cells["I" + index].Value = item.Total;
                index++;
            }
            dataSheet.Cells.AutoFitColumns();
        }



        #endregion

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

                case ReportType.ServerAvailableHoursTemplate:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\ServerAvailableHoursTemplate.xlsx";
                    break;

                case ReportType.StaffPercentagesMonthly:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\StaffPercentagesMonthlyReportTemplate.xlsx";
                    break;

                case ReportType.CategoryPercentagesMonthly:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\CategoryPercentagesMonthlyReportTemplate.xlsx";
                    break;

                case ReportType.ServerSalariesMonthly:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\ServerSalariesMonthlyReportTemplate.xlsx";
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