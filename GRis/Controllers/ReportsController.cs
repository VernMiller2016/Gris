using Gris.Application.Core;
using Gris.Application.Core.Interfaces;
using GRis.ViewModels.Reports;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ReportsController : Controller
    {
        private IProgramService _programService;
        private IServerTimeEntryService _serverTimeEntryService;
        private IPaySourceService _paySourceService;
        private IExportingService _exportingService;

        public ReportsController(IProgramService programService, IServerTimeEntryService serverTimeEntryService, IPaySourceService paySourceService, IExportingService exportingService)
        {
            _programService = programService;
            _serverTimeEntryService = serverTimeEntryService;
            _paySourceService = paySourceService;
            _exportingService = exportingService;
        }

        // GET: Reports
        public ActionResult Index()
        {
            var viewmodel = new ReportFiltersViewModel() { };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ReportFiltersViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ReportData = _serverTimeEntryService.GetServerTimeEntriesMonthlyReport(viewmodel.SelectedDate.Value).ToList();
            }
            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult ExportServerTimeEntriesMonthlyReportToExcel(ReportFiltersViewModel viewmodel)
        {
            var report = _exportingService.GetServerTimeEntriesMonthlyReportExcel(viewmodel.SelectedDate.Value);
            MemoryStream stream = _exportingService.GetServerTimeEntriesMonthlyReportExcel(viewmodel.SelectedDate.Value);

            return File(stream, Constants.ExcelFilesMimeType,
                string.Format(Constants.ServerTimeEntriesMonthlyReportExcelFileName
                , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(viewmodel.SelectedDate.Value.Month)
                , viewmodel.SelectedDate.Value.Year));
        }
    }
}