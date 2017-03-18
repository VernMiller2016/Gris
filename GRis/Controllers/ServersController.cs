using Gris.Application.Core.Interfaces;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ServersController : Controller
    {
        private IServerService _serverService;
        public ServersController(IServerService serverService)
        {
            _serverService = serverService;
        }

        // GET: Servers
        public ActionResult Index()
        {
            var servers = _serverService.GetServers();
            return View(servers.ToList());
        }

        // GET: Servers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = _serverService.GetServerById(id.Value);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // GET: Servers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServerId,FirstName,LastName,Active")] Server server)
        {
            if (ModelState.IsValid)
            {
                _serverService.AddServer(server);

                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(server);
        }

        // GET: Servers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Server server = _serverService.GetServerById(id.Value);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServerId,FirstName,LastName,Active")] Server server)
        {
            if (ModelState.IsValid)
            {
               _serverService.UpdateServer(server);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(server);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = _serverService.GetServerById(id.Value);
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
            Server server = _serverService.GetServerById(id);
            if (server != null) _serverService.Remove(server);
            //db.SaveChanges();
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
                    List<Server> addedServers = new List<Models.Server>();
                    viewmodel.ExcelFile.SaveAs(path); // save a copy of the uploaded file.
                    // convert the uploaded file into datatable, then add/update db entities.
                    var dtServers = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true);
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var server = new Server()
                        {
                            ServerId = int.Parse(row["Staff"].ToString()),
                            // some columns does not have ',' separater.
                            FirstName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[1],
                            LastName = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            Active = row["active"].ToString() == "Y" ? true : false
                        };
                        //check if server does not exist
                        if(server.ServerId != 0)
                        {
                            if (_serverService.GetServerById(server.ServerId) == null)
                            {
                                addedServers.Add(server);
                            }
                            else
                            {
                                _serverService.UpdateServer(server);
                            }
                        }
                    }
                    _serverService.AddServers(addedServers);
                }
            }

            return View(viewmodel);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}