using Gris.Application.Core;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Contracts.Reports;
using Gris.Application.Core.Interfaces;
using GRis.Extensions;
using GRis.ViewModels.Reports;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GRis.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private IProgramService _programService;
        private IServerTimeEntryService _serverTimeEntryService;
        private IPaySourceService _paySourceService;
        private IExportingService _exportingService;
        private IServerSalaryReportService _serverSalaryReportService;

        public ReportsController(IProgramService programService, IServerTimeEntryService serverTimeEntryService, IPaySourceService paySourceService, IExportingService exportingService,IServerSalaryReportService serverSalaryReportService)
        {
            _programService = programService;
            _serverTimeEntryService = serverTimeEntryService;
            _paySourceService = paySourceService;
            _exportingService = exportingService;
            _serverSalaryReportService = serverSalaryReportService;
        }

        public ActionResult ServerTimeEntriesMonthlyReport(ReportFilterViewModel filter, int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entities = Enumerable.Empty<ServerTimeEntriesMonthlyReportEntity>();
            if (TryValidateModel(filter))
            {
                entities = _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(filter.Date.Value, pagingInfo);
                ViewBag.DisplayResults = true;
            }
            else
            {
                ViewBag.DisplayResults = false;
            }
            ViewBag.FilterViewModel = filter;

            var viewmodel = entities.ToManualPagedList(pagingInfo);
            return View(viewmodel);
        }

        public ActionResult StaffPercentagesMonthlyReport()
        {
            ReportFilterViewModel viewmodel = new ReportFilterViewModel();
            return View(viewmodel);
        }

        public ActionResult CategoryPercentagesMonthlyReport()
        {
            ReportFilterViewModel viewmodel = new ReportFilterViewModel();
            return View(viewmodel);
        }

        public ActionResult ServerSalariesMonthlyReport(ReportFilterViewModel filter, int page = 1)
        {
            var entities = Enumerable.Empty<ServerSalaryReportViewModel>();
            if (TryValidateModel(filter))
            {
                entities = _serverSalaryReportService.GetServerSalaryMonthlyReport(filter.Date.Value);
                ViewBag.DisplayResults = true;
            }
            else
            {
                ViewBag.DisplayResults = false;
            }
            ViewBag.FilterViewModel = filter;

            return View(entities);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportStaffPercentagesMonthlyReportToExcel(ReportFilterViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                MemoryStream stream = _exportingService.GetStaffPercentagesMonthlyReportExcel(viewmodel.Date.Value);

                return File(stream, Constants.ExcelFilesMimeType,
                    string.Format(Constants.StaffPercentagesMonthlyReportExcelFileName
                    , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewmodel.Date.Value.Month)
                    , viewmodel.Date.Value.Year));
            }
            return View("StaffPercentagesMonthlyReport", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult ExportServerTimeEntriesMonthlyReportToExcel(ReportFilterViewModel viewmodel)
        {
            MemoryStream stream = _exportingService.GetServerTimeEntriesMonthlyReportExcel(viewmodel.Date.Value);

            return File(stream, Constants.ExcelFilesMimeType,
                string.Format(Constants.ServerTimeEntriesMonthlyReportExcelFileName
                , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewmodel.Date.Value.Month)
                , viewmodel.Date.Value.Year));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult ExportServerSalariesMonthlyReportToExcel(ReportFilterViewModel viewmodel)
        {
            MemoryStream stream = _exportingService.GetServerSalariesMothlyReportExcel(viewmodel.Date.Value);

            return File(stream, Constants.ExcelFilesMimeType,
                string.Format(Constants.ServerTimeEntriesMonthlyReportExcelFileName
                , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewmodel.Date.Value.Month)
                , viewmodel.Date.Value.Year));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportCategoryPercentagesMonthlyReportToExcel(ReportFilterViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                MemoryStream stream = _exportingService.GetCategoryPercentagesMonthlyReportExcel(viewmodel.Date.Value);

                return File(stream, Constants.ExcelFilesMimeType,
                    string.Format(Constants.CategoryPercentagesMonthlyReportExcelFileName
                    , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewmodel.Date.Value.Month)
                    , viewmodel.Date.Value.Year));
            }
            return View("CategoryPercentagesMonthlyReport", viewmodel);
        }
    }
}