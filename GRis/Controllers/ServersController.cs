using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Extensions;
using GRis.ViewModels.General;
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
    public class ServersController : Controller
    {
        private IServerService _serverService;

        public ServersController(IServerService serverService)
        {
            _serverService = serverService;
        }

        // GET: Servers
        public ActionResult Index(int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entites = _serverService.GetServers(pagingInfo);
            var viewmodel = entites.ToMappedPagedList<Server, ServerDetailsViewModel>(pagingInfo);
            return View(viewmodel);
        }

        // GET: Servers/Details/5
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

        // GET: Servers/Create
        public ActionResult Create()
        {
            var viewmodel = new ServerAddViewModel();
            return View(viewmodel);
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<ServerAddViewModel, Server>(viewmodel);
                _serverService.AddServer(entity);
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }

        // GET: Servers/Edit/5
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
            return View(viewmodel);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Index");
            }
            return View(viewmodel);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = _serverService.GetById(id.Value);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Server server = _serverService.GetById(id);
            if (server != null) _serverService.Remove(server);
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
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var entityViewModel = new ServerAddViewModel()
                        {
                            VendorId = int.Parse(row["Staff"].ToString()),
                            // some columns does not have ',' separater.
                            FirstName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[1],
                            LastName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            Active = row["active"].ToString() == "Y" ? true : false,
                            CategoryId = CategoryConverter.ConvertFromCategoryNameToId(row["Category"].ToString())
                        };
                        //check if server does not exist
                        if (entityViewModel.VendorId != 0)
                        {
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
                            }
                        }
                    }
                    if (addedServers.Any())
                    {
                        _serverService.AddServers(addedServers);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }
    }
}