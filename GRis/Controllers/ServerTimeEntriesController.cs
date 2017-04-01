using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.ViewModels.General;
using GRis.ViewModels.ServerTimeEntry;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GRis.Controllers
{
    [Authorize]
    public class ServerTimeEntriesController : Controller
    {
        private IServerTimeEntryService _serverTimeEntryService;
        private IServerService _serverService;
        private IPaySourceService _paySourceService;

        public ServerTimeEntriesController(IServerTimeEntryService serverTimeEntryService, IServerService serverService, IPaySourceService paySourceService)
        {
            _serverTimeEntryService = serverTimeEntryService;
            _serverService = serverService;
            _paySourceService = paySourceService;
        }

        // GET: ServerTimeEntries
        public ActionResult Index(int? page)
        {
            //var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var timeEntries = _serverTimeEntryService.GetServerTimeEntries();

            //var pageData = timeEntries.ToPagedList(pageNumber, 25);
            return View(timeEntries);
        }

        public ActionResult Upload()
        {
            return View(new UploadedExcelSheetViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(UploadedExcelSheetViewModel viewmodel)
        {
            if (ModelState.IsValid) // validate file exist
            {
                if (viewmodel.ExcelFile != null && viewmodel.ExcelFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(viewmodel.ExcelFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/ServerTimeEntries/"), DateTime.Now.GetTimeStamp() + "_" + fileName);
                    List<ServerTimeEntry> timeEntries = new List<ServerTimeEntry>();
                    viewmodel.ExcelFile.SaveAs(path); // save a copy of the uploaded file.
                    // convert the uploaded file into datatable, then add/update db entities.
                    var dtServers = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true);
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var timeEntryViewModel = new ServerTimeEntryAddViewModel()
                        {
                            ServerVendorId = int.Parse(row["Server ID"].ToString()),
                            PaySourceVendorId = int.Parse(row["Current Pay Source"].ToString()),
                            BeginDate = DateTime.Parse(row["Begin Date"].ToString()),
                            Duration = TimeSpan.Parse(row["Duration"].ToString())
                        };
                        var existedServer = _serverService.GetByVendorId(timeEntryViewModel.ServerVendorId);
                        if (existedServer == null)
                        {
                            ModelState.AddModelError("", $"Invalid Server Id with value ={timeEntryViewModel.ServerVendorId}");
                        }

                        var existedPaySource = _paySourceService.GetByVendorId(timeEntryViewModel.PaySourceVendorId);
                        if (existedPaySource == null)
                        {
                            ModelState.AddModelError("", $"Invalid PaySource Id with value ={timeEntryViewModel.PaySourceVendorId}");
                        }

                        timeEntries.Add(new ServerTimeEntry()
                        {
                            Server = existedServer,
                            PaySource = existedPaySource,
                            BeginDate = timeEntryViewModel.BeginDate,
                            Duration = timeEntryViewModel.Duration
                        });
                    }
                    if (ModelState.Keys.Any())
                    {
                        return View(viewmodel);
                    }
                    else
                    {
                        _serverTimeEntryService.AddServerTimeEntries(timeEntries);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }
    }
}