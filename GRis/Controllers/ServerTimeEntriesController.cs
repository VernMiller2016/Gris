using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Extensions;
using GRis.ViewModels.Common;
using GRis.ViewModels.ServerTimeEntry;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    [Authorize]
    public class ServerTimeEntriesController : BaseController
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
        public ActionResult Index(ServerTimeEntryFilterViewModel filter, string search, string option, int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entities = Enumerable.Empty<ServerTimeEntry>();
            if (filter != null && filter.Date.HasValue)
            {
                entities = _serverTimeEntryService.GetServerTimeEntries(filter.Date.Value, pagingInfo);
            }
            else
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
                {
                    pagingInfo.SearchOption = option;
                    pagingInfo.SearchValue = search;
                }
                entities = _serverTimeEntryService.GetServerTimeEntries(pagingInfo);
            }

            ViewBag.FilterViewModel = filter;

            var viewmodel = entities.ToMappedPagedList<ServerTimeEntry, ServerTimeEntryDetailsViewModel>(pagingInfo);
            return View(viewmodel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _serverTimeEntryService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<ServerTimeEntry, ServerTimeEntryDetailsViewModel>(entity);
            return View(viewmodel);
        }

        public ActionResult Create()
        {
            var viewmodel = new ServerTimeEntryAddViewModel();
            viewmodel.SelectedServers = _serverService.GetServers().Select(t => new SelectListItem()
            {
                Text = t.FullName,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPaySources = _paySourceService.GetPaySources().Select(t => new SelectListItem()
            {
                Text = t.Description,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPrograms = new List<SelectListItem>();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerTimeEntryAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<ServerTimeEntryAddViewModel, ServerTimeEntry>(viewmodel);
                _serverTimeEntryService.AddServerTimeEntry(entity);

                Success($"<strong>Time Entry</strong> was successfully added.");
                return RedirectToAction("Index");
            }

            viewmodel.SelectedServers = _serverService.GetServers().Select(t => new SelectListItem()
            {
                Text = t.FullName,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPaySources = _paySourceService.GetPaySources().Select(t => new SelectListItem()
            {
                Text = t.Description,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPrograms = new List<SelectListItem>();
            return View(viewmodel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ServerTimeEntry entity = _serverTimeEntryService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }

            var viewmodel = Mapper.Map<ServerTimeEntry, ServerTimeEntryEditViewModel>(entity);
            viewmodel.SelectedServers = _serverService.GetServers().Select(t => new SelectListItem()
            {
                Text = t.FullName,
                Value = t.Id.ToString()
            });
            var paysourceList = _paySourceService.GetPaySources();
            viewmodel.SelectedPaySources = paysourceList.Select(t => new SelectListItem()
            {
                Text = t.Description,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPrograms = paysourceList.FirstOrDefault(t => t.Id == entity.PaySourceId).Programs.Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServerTimeEntryEditViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                ServerTimeEntry entity = _serverTimeEntryService.GetById(viewmodel.Id);
                if (entity == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(viewmodel, entity);
                _serverTimeEntryService.UpdateServerTimeEntry(entity, viewmodel.ApplyProgramUpdateToAllEntries);

                Success($"<strong>Time Entry</strong> was successfully updated.");
                return RedirectToAction("Index");
            }
            viewmodel.SelectedServers = _serverService.GetServers().Select(t => new SelectListItem()
            {
                Text = t.FullName,
                Value = t.Id.ToString()
            });
            var paysourceList = _paySourceService.GetPaySources();
            viewmodel.SelectedPaySources = paysourceList.Select(t => new SelectListItem()
            {
                Text = t.Description,
                Value = t.Id.ToString()
            });
            viewmodel.SelectedPrograms = paysourceList.FirstOrDefault(t => t.Id == viewmodel.PaySourceId).Programs.Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            return View(viewmodel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServerTimeEntry entity = _serverTimeEntryService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<ServerTimeEntry, ServerTimeEntryDetailsViewModel>(entity);
            return View(viewmodel);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServerTimeEntry entity = _serverTimeEntryService.GetById(id);
            if (entity != null) _serverTimeEntryService.Remove(entity);
            Success($"<strong>Time Entry</strong> was successfully deleted.");
            return RedirectToAction("Index");
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
                    var columnsToImport = new string[] { "Server ID", "Current Pay Source", "Begin Date", "Duration" };
                    var dtServers = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true, columnsToImport);
                    var invalidServers = new List<int>();
                    var invalidPaysources = new List<int>();
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var timeEntryViewModel = new ServerTimeEntryAddViewModel()
                        {
                            ServerId = int.Parse(row["Server ID"].ToString()),
                            PaySourceId = int.Parse(row["Current Pay Source"].ToString()),
                            BeginDate = DateTime.Parse(row["Begin Date"].ToString()),
                            Duration = TimeSpan.Parse(row["Duration"].ToString())
                        };
                        var existedServer = _serverService.GetByVendorId(timeEntryViewModel.ServerId);
                        if (existedServer == null)
                        {
                            if (!invalidServers.Any(t => t == timeEntryViewModel.ServerId))
                                invalidServers.Add(timeEntryViewModel.ServerId);
                        }

                        var existedPaySource = _paySourceService.GetByVendorId(timeEntryViewModel.PaySourceId);
                        if (existedPaySource == null)
                        {
                            if (!invalidPaysources.Any(t => t == timeEntryViewModel.PaySourceId))
                                invalidPaysources.Add(timeEntryViewModel.PaySourceId);
                        }

                        if (existedServer != null && existedPaySource != null)
                        {
                            // check if entity already exists.
                            var entity = Mapper.Map<ServerTimeEntryAddViewModel, ServerTimeEntry>(timeEntryViewModel);
                            entity.ServerId = existedServer.Id;
                            entity.PaySourceId = existedPaySource.Id;
                            entity.ProgramId = existedPaySource.Programs.Any() ? existedPaySource.Programs.ToList()[0].Id : (int?)null;
                            if (!_serverTimeEntryService.TimeEntryExists(entity))
                                timeEntries.Add(entity);
                        }
                    }
                    if (invalidServers.Any() || invalidPaysources.Any())
                    {
                        invalidServers.ForEach(invalidServerId => { ModelState.AddModelError("", $"Invalid Server Id with value ={invalidServerId}"); });
                        invalidPaysources.ForEach(invalidPaysource => { ModelState.AddModelError("", $"Invalid PaySource Id with value ={invalidPaysource}"); });
                        return View(viewmodel);
                    }
                    else
                    {
                        _serverTimeEntryService.AddServerTimeEntries(timeEntries);
                    }
                    Success($"<strong>{timeEntries.Count}</strong> Time Entries have been successfully added. <br\\>"
                        + $"<strong>{dtServers.Rows.Count - timeEntries.Count}</strong> Time Entries are duplicated and have been skipped.");
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }
    }
}