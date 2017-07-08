using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Extensions;
using GRis.ViewModels.Common;
using GRis.ViewModels.Server;
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
    public class ServersController : BaseController
    {
        private IServerService _serverService;
        private IElementService _elementService;

        public ServersController(IServerService serverService, IElementService elementService)
        {
            _serverService = serverService;
            _elementService = elementService;
        }

        public ActionResult Index(string search, string option, int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(option))
            {
                pagingInfo.SearchOption = option;
                pagingInfo.SearchValue = search;
            }
            var entites = _serverService.GetServers(pagingInfo);
            var viewmodel = entites.ToMappedPagedList<Server, ServerDetailsViewModel>(pagingInfo);
            return View(viewmodel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _serverService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Server, ServerDetailsViewModel>(entity);

            return View(viewmodel);
        }

        public ActionResult Create()
        {
            var viewmodel = new ServerAddViewModel();
            viewmodel.AvailableCategories = _serverService.GetCategories().Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();

            viewmodel.AvailableElements = _elementService.GetElements().Select(t => new SelectListItem()
            {
                Text = t.DisplayName,
                Value = t.Id.ToString()
            }).ToList();

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                // check if vendor id already exists.
                if (_serverService.GetByVendorId(viewmodel.VendorId) == null)
                {
                    var entity = Mapper.Map<ServerAddViewModel, Server>(viewmodel);
                    _serverService.AddServer(entity);

                    Success($"<strong>{entity.FullName}</strong> was successfully added.");
                    return RedirectToAction("Index");
                }
                else
                {
                    Danger($"A server with same Id <strong>{viewmodel.VendorId}</strong> already exists.");
                }
            }

            viewmodel.AvailableCategories = _serverService.GetCategories().Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();

            viewmodel.AvailableElements = _elementService.GetElements().Select(t => new SelectListItem()
            {
                Text = t.DisplayName,
                Value = t.Id.ToString()
            }).ToList();

            return View(viewmodel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Server entity = _serverService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Server, ServerEditViewModel>(entity);
            viewmodel.CategoryId = entity.CategoryId;
            viewmodel.AvailableCategories = _serverService.GetCategories().Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();

            viewmodel.AvailableElements = _elementService.GetElements().Select(t => new SelectListItem()
            {
                Text = t.DisplayName,
                Value = t.Id.ToString()
            }).ToList();

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServerEditViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                Server entity = _serverService.GetById(viewmodel.Id);
                if (entity == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(viewmodel, entity);

                _serverService.UpdateServer(entity);
                Success($"<strong>{entity.FullName}</strong> was successfully updated.");
                return RedirectToAction("Index");
            }
            viewmodel.AvailableCategories = _serverService.GetCategories().Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();

            viewmodel.AvailableElements = _elementService.GetElements().Select(t => new SelectListItem()
            {
                Text = t.DisplayName,
                Value = t.Id.ToString()
            }).ToList();

            return View(viewmodel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _serverService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Server, ServerDetailsViewModel>(entity);
            return View(viewmodel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Server entity = _serverService.GetById(id);
            if (entity != null) _serverService.Remove(entity);
            Success($"<strong>{entity.FullName}</strong> was successfully deleted.");
            return RedirectToAction("Index");
        }

        [HttpGet]
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
                    var path = Path.Combine(Server.MapPath("~/Uploads/Servers/"), DateTime.Now.GetTimeStamp() + "_" + fileName);
                    List<Server> addedServers = new List<Server>();
                    viewmodel.ExcelFile.SaveAs(path); // save a copy of the uploaded file.
                    // convert the uploaded file into datatable, then add/update db entities.
                    var dtServers = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true);
                    int numOfServersUpdated = 0;
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var entityViewModel = new ServerAddViewModel()
                        {
                            VendorId = int.Parse(row["Staff"].ToString()),
                            // some columns does not have ',' separater.
                            FirstName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[1],
                            LastName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            GpEmpNumber = !string.IsNullOrWhiteSpace(row["Gp Emp #"].ToString()) ? row["Gp Emp #"].ToString() : null,
                            ElementId = !string.IsNullOrWhiteSpace(row["Element"].ToString()) ? int.Parse(row["Element"].ToString()) : (int?)null,
                            Active = row["active"].ToString() == "Y" ? true : false,
                            CategoryId = CategoryConverter.ConvertFromCategoryNameToId(row["Category"].ToString())
                        };
                        //check if server does not exist
                        if (entityViewModel.VendorId != 0)
                        {
                            if (entityViewModel.ElementId.HasValue)
                            {
                                var existedElement = _elementService.GetByVendorId(entityViewModel.ElementId.Value);
                                if (existedElement == null)
                                {
                                    Danger($"Invalid Element Id with value ={entityViewModel.ElementId.Value}");
                                    continue;
                                }
                            }

                            var existedEntity = _serverService.GetByVendorId(entityViewModel.VendorId);
                            if (existedEntity == null)
                            {
                                var entity = Mapper.Map<ServerAddViewModel, Server>(entityViewModel);
                                addedServers.Add(entity);
                            }
                            else
                            {
                                Mapper.Map(entityViewModel, existedEntity);
                                _serverService.UpdateServer(existedEntity);
                                numOfServersUpdated++;
                            }
                        }
                    }
                    if (addedServers.Any())
                    {
                        _serverService.AddServers(addedServers);
                    }
                    Success($"<strong>{addedServers.Count}</strong> servers have been successfully added. <br\\>"
                        + $"<strong>{numOfServersUpdated}</strong> servers have been successfully updated.");
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }
    }
}