using AutoMapper;
using Gris.Application.Core;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Extensions;
using GRis.ViewModels.ServerAvailableHourModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ServerAvailableHoursController : BaseController
    {
        private IServerAvailableHourService _serverAvailableHourService;
        private IServerService _serverService;
        private IExportingService _exportingService;

        public ServerAvailableHoursController(IServerAvailableHourService serverAvailableHourService, IExportingService exportingService, IServerService serverService)
        {
            _serverAvailableHourService = serverAvailableHourService;
            _exportingService = exportingService;
            _serverService = serverService;
        }

        public ActionResult Index(ServerAvailableHourFilterViewModel filter, int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entities = Enumerable.Empty<ServerAvailableHour>();
            if (TryValidateModel(filter))
            {
                entities = _serverAvailableHourService.GetByDate(filter.Date.Value, pagingInfo);
                ViewBag.DisplayResults = true;
            }
            else
            {
                ViewBag.DisplayResults = false;
            }

            var viewmodel = new ServerAvailableHourListViewModel()
            {
                Filters = filter,
                Data = entities.ToMappedPagedList<ServerAvailableHour, ServerAvailableHourDetailsViewModel>(pagingInfo)
            };
            return View(viewmodel);
        }

        [HttpGet]
        public FileResult ExportServerAvailableHoursTemplate(ServerAvailableHourUploadViewModel viewmodel)
        {
            DateTime dateForAvailableHours = viewmodel.Date.HasValue ? viewmodel.Date.Value : DateTime.Now;
            MemoryStream stream = _exportingService.GetServerAvailableHoursTemplate(viewmodel.DefaultAvailableHours, dateForAvailableHours);

            return File(stream, Constants.ExcelFilesMimeType,
                string.Format(Constants.ServerAvailableHoursTemplateExcelFileName
                , CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateForAvailableHours.Month)
                , dateForAvailableHours.Year));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(ServerAvailableHourUploadViewModel viewmodel)
        {
            if (ModelState.IsValid) // validate file exist
            {
                if (viewmodel.ExcelFile != null && viewmodel.ExcelFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(viewmodel.ExcelFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/AvailableHours/"), DateTime.Now.GetTimeStamp() + "_" + fileName);
                    List<ServerAvailableHour> addedEntities = new List<ServerAvailableHour>();
                    viewmodel.ExcelFile.SaveAs(path); // save a copy of the uploaded file.
                    // convert the uploaded file into datatable, then add/update db entities.
                    var dtAvailableHours = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true);
                    int numOfEntitiesUpdated = 0;
                    // load existed entities from DB, aka "cache".
                    var existedEntities = GetExistedAvailableHours(dtAvailableHours);
                    foreach (var row in dtAvailableHours.AsEnumerable().ToList())
                    {
                        var entityViewModel = new ServerAvailableHourAddViewModel()
                        {
                            ServerVendorId = int.Parse(row["Server ID"].ToString()),
                            DateRange = DateTime.Parse(row["Date"].ToString()),
                            AvailableHours = float.Parse(row["AvailableHours"].ToString())
                        };
                        var existedServer = _serverService.GetByVendorId(entityViewModel.ServerVendorId);
                        if (existedServer == null)
                        {
                            ModelState.AddModelError("", $"Invalid Server Id with value ={entityViewModel.ServerVendorId}");
                        }
                        // check if entity already exists.
                        var existedEntity = existedEntities.FirstOrDefault(t => t.Server.VendorId == entityViewModel.ServerVendorId
                         && t.DateRange.Year == entityViewModel.DateRange.Year && t.DateRange.Month == entityViewModel.DateRange.Month);
                        if (existedEntity == null)
                        {
                            var entity = Mapper.Map<ServerAvailableHourAddViewModel, ServerAvailableHour>(entityViewModel);
                            entity.ServerId = existedServer.Id;
                            addedEntities.Add(entity);
                        }
                        else
                        {
                            Mapper.Map(entityViewModel, existedEntity);
                            _serverAvailableHourService.Update(existedEntity);
                            numOfEntitiesUpdated++;
                        }
                    }
                    if (addedEntities.Any())
                    {
                        _serverAvailableHourService.Add(addedEntities);
                    }
                    Success($"<strong>{addedEntities.Count}</strong> records have been successfully added. <br\\>"
                        + $"<strong>{numOfEntitiesUpdated}</strong> records have been successfully updated.");
                }
            }

            return RedirectToAction("Index");
        }

        #region Helpers

        private IEnumerable<ServerAvailableHour> GetExistedAvailableHours(DataTable dtAvailableHours)
        {
            var groupByDate = dtAvailableHours.AsEnumerable().GroupBy(t => t["Date"]);
            foreach (var item in groupByDate)
            {
                var entities = _serverAvailableHourService.GetByDate(DateTime.Parse(item.Key.ToString()));
                foreach (var entity in entities)
                {
                    yield return entity;
                }
            }
        }

        #endregion Helpers
    }
}