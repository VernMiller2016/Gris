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

       public  MemoryStream GetServerSalariesPercentageMothlyReportExcel(DateTime time)
        {
            string excelTemplate = GetExcelTemplate(ReportType.ServerSalariesPercentageMonthly);
            var templateFile = new FileInfo(excelTemplate);
            ExcelPackage package = new ExcelPackage(templateFile, true);

            GenerateServerSalariesPercentageMonthlyReportExcel(package, _serverSalariesService.GetServerSalaryMonthlyPercentageReport(time), time);

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
        private void GenerateServerSalariesPercentageMonthlyReportExcel(ExcelPackage excelPackage,IEnumerable<ServerSalaryReportViewModel> reportData,DateTime time)
        {
            var dataSheet = excelPackage.Workbook.Worksheets[1];
            var salarySheet = excelPackage.Workbook.Worksheets["Salary"];
            var tempHelpSheet = excelPackage.Workbook.Worksheets["TempHelp"];
            var overtimeSheet = excelPackage.Workbook.Worksheets["Overtime"];
            var retirementSheet = excelPackage.Workbook.Worksheets["Retirement"];
            var socialSecuritySheet = excelPackage.Workbook.Worksheets["SocialSecurity"];
            var medicalAndLifeInsSheet = excelPackage.Workbook.Worksheets["MedicalAndLifeIns"];
            var industInsSheet = excelPackage.Workbook.Worksheets["IndustIns"];
            var index = 2; // starting index of each sheet.
            var salaryWorkSheetIndex = 2;
            var tempHelpWorkSheetIndex = 2;
            var overTimeWorkSheetIndex = 2;
            var retirementWorkSheetIndex = 2;
            var socialSecurityWorkSheetIndex = 2;
            var medicalAndLifeInsWorkSheetIndex = 2;
            var industInsWorkSheetIndex = 2;
            foreach (var item in reportData)
            {
                if(item.Programs!= null)
                {
                    if(item.SalaryAccount != null)
                    {
                        salarySheet.Cells["A" + salaryWorkSheetIndex].Value = item.ServerName;
                        salarySheet.Cells["B" + salaryWorkSheetIndex].Value = item.SalaryAccount.Value;
                        var currentCell = salarySheet.Cells["B" + salaryWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            salarySheet.Cells[1,currentColumn+1].Value = program.ProgramName;
                            salarySheet.Cells[currentRow,currentColumn+1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.SalaryAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        salaryWorkSheetIndex++;
                    }
                    if(item.TempHelpAccount != null)
                    {
                        tempHelpSheet.Cells["A" + tempHelpWorkSheetIndex].Value = item.ServerName;
                        tempHelpSheet.Cells["B" + tempHelpWorkSheetIndex].Value = item.TempHelpAccount.Value;
                        var currentCell = tempHelpSheet.Cells["B" + tempHelpWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            tempHelpSheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            tempHelpSheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.TempHelpAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        tempHelpWorkSheetIndex++;
                    }
                    if(item.OverTimeAccount != null)
                    {
                        overtimeSheet.Cells["A" + overTimeWorkSheetIndex].Value = item.ServerName;
                        overtimeSheet.Cells["B" + overTimeWorkSheetIndex].Value = item.OverTimeAccount.Value;
                        var currentCell = overtimeSheet.Cells["B" + overTimeWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            overtimeSheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            overtimeSheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.OverTimeAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        overTimeWorkSheetIndex++;
                    }
                    if (item.RetirementAccount != null)
                    {
                        retirementSheet.Cells["A" + retirementWorkSheetIndex].Value = item.ServerName;
                        retirementSheet.Cells["B" + retirementWorkSheetIndex].Value = item.RetirementAccount.Value;
                        var currentCell = retirementSheet.Cells["B" + retirementWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            retirementSheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            retirementSheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.RetirementAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        retirementWorkSheetIndex++;
                    }

                    if (item.SocialSecurityAccount != null)
                    {
                        socialSecuritySheet.Cells["A" + socialSecurityWorkSheetIndex].Value = item.ServerName;
                        socialSecuritySheet.Cells["B" + socialSecurityWorkSheetIndex].Value = item.SocialSecurityAccount.Value;
                        var currentCell = socialSecuritySheet.Cells["B" + socialSecurityWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            socialSecuritySheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            socialSecuritySheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.SocialSecurityAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        socialSecurityWorkSheetIndex++;
                    }

                    if (item.MedicalAndLifeInsuranceAccount != null)
                    {
                        medicalAndLifeInsSheet.Cells["A" + medicalAndLifeInsWorkSheetIndex].Value = item.ServerName;
                        medicalAndLifeInsSheet.Cells["B" + medicalAndLifeInsWorkSheetIndex].Value = item.MedicalAndLifeInsuranceAccount.Value;
                        var currentCell = medicalAndLifeInsSheet.Cells["B" + medicalAndLifeInsWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            medicalAndLifeInsSheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            medicalAndLifeInsSheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.MedicalAndLifeInsuranceAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        medicalAndLifeInsWorkSheetIndex++;
                    }

                    if (item.IndustrialInsuranceAccount != null)
                    {
                        industInsSheet.Cells["A" + industInsWorkSheetIndex].Value = item.ServerName;
                        industInsSheet.Cells["B" + industInsWorkSheetIndex].Value = item.IndustrialInsuranceAccount.Value;
                        var currentCell = industInsSheet.Cells["B" + industInsWorkSheetIndex];
                        int currentColumn = currentCell.Start.Column;
                        int currentRow = currentCell.Start.Row;
                        foreach (var program in item.Programs)
                        {
                            industInsSheet.Cells[1, currentColumn + 1].Value = program.ProgramName;
                            industInsSheet.Cells[currentRow, currentColumn + 1].Value = item.Programs.Sum(t => t.Duration.TotalHours)!=0?
                                item.IndustrialInsuranceAccount.Value
                                * (decimal)(program.Duration.TotalHours / item.Programs.Sum(t => t.Duration.TotalHours)):0;
                            currentColumn++;
                        }
                        industInsWorkSheetIndex++;
                    }

                }
                dataSheet.Cells["A" + index].Value = item.ServerName;
                dataSheet.Cells["B" + index].Value = item.SalaryAccount != null ? item.SalaryAccount.ValueInPercentage : 0;
                dataSheet.Cells["C" + index].Value = item.TempHelpAccount != null ? item.TempHelpAccount.ValueInPercentage : 0;
                dataSheet.Cells["D" + index].Value = item.OverTimeAccount != null ? item.OverTimeAccount.ValueInPercentage : 0;
                dataSheet.Cells["E" + index].Value = item.RetirementAccount != null ? item.RetirementAccount.ValueInPercentage : 0;
                dataSheet.Cells["F" + index].Value = item.SocialSecurityAccount != null ? item.SocialSecurityAccount.ValueInPercentage : 0;
                dataSheet.Cells["G" + index].Value = item.MedicalAndLifeInsuranceAccount != null ? item.MedicalAndLifeInsuranceAccount.ValueInPercentage : 0;
                dataSheet.Cells["H" + index].Value = item.IndustrialInsuranceAccount != null ? item.IndustrialInsuranceAccount.ValueInPercentage : 0;
                dataSheet.Cells["I" + index].Value = item.TotalInPercentage;
                index++;
            }
            dataSheet.Cells.AutoFitColumns();
            salarySheet.Cells.AutoFitColumns();
            industInsSheet.Cells.AutoFitColumns();
            medicalAndLifeInsSheet.Cells.AutoFitColumns();
            overtimeSheet.Cells.AutoFitColumns();
            retirementSheet.Cells.AutoFitColumns();
            socialSecuritySheet.Cells.AutoFitColumns();
            tempHelpSheet.Cells.AutoFitColumns();
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
                case ReportType.ServerSalariesPercentageMonthly:
                    templatePath = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\ExcelTemplates\\ServerSalariesPercentageMonthlyReportTemplate.xlsx";
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