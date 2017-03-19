using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class PaySourcesController : Controller
    {
        private IPaySourceService _paySourceService;

        public PaySourcesController(IPaySourceService paySourceService)
        {
            _paySourceService = paySourceService;
        }

        // GET: PaySources
        public ActionResult Index()
        {
            var paySources = _paySourceService.GetPaySources();
            return View(paySources.ToList());
        }

        // GET: PaySources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var paySource = _paySourceService.GetById(id.Value);
            if (paySource == null)
            {
                return HttpNotFound();
            }
            return View(paySource);
        }

        // GET: PaySources/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description");
        //    return View();
        //}

        public ActionResult Create()
        {
            return View();
        }

        // POST: PaySources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PaySourceId,Description,Active,ProgramId")] PaySource paySource)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PaySources.Add(paySource);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
        //    return View(paySource);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaySourceId,Description,Active,ProgramId")] PaySource paySource)
        {
            if (ModelState.IsValid)
            {
                _paySourceService.AddPaySource(paySource);

                return RedirectToAction("Index");
            }

            //ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // GET: PaySources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource paySource = _paySourceService.GetById(id.Value);
            if (paySource == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // POST: PaySources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PaySourceId,Description,Active,ProgramId")] PaySource paySource)
        {
            if (ModelState.IsValid)
            {
                PaySource existedPaySource = _paySourceService.GetById(paySource.Id);
                if (existedPaySource == null)
                {
                    return HttpNotFound();
                }
                // ToDo: user automapper to automatically update model from viewmodel.
                existedPaySource.Description = paySource.Description;
                existedPaySource.Active = paySource.Active;
                existedPaySource.ProgramId = paySource.ProgramId;

                _paySourceService.UpdatePaySource(existedPaySource);
                return RedirectToAction("Index");
            }
            //ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // GET: PaySources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource paySource = _paySourceService.GetById(id.Value);
            if (paySource == null)
            {
                return HttpNotFound();
            }
            return View(paySource);
        }

        // POST: PaySources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaySource paySource = _paySourceService.GetById(id);
            if (paySource != null) _paySourceService.Remove(paySource);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
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
                    var path = Path.Combine(Server.MapPath("~/Uploads/PaySources/"), DateTime.Now.GetTimeStamp() + "_" + fileName);
                    List<PaySource> addedPaySources = new List<PaySource>();
                    viewmodel.ExcelFile.SaveAs(path); // save a copy of the uploaded file.
                    // convert the uploaded file into datatable, then add/update db entities.
                    var dtServers = ImportUtils.ImportXlsxToDataTable(viewmodel.ExcelFile.InputStream, true);
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var paySource = new PaySource()
                        {
                            PaySourceId = int.Parse(row["PaySourceId"].ToString()),
                            // some columns does not have ',' separater.
                            Description = row["Description"].ToString(),
                            //ProgramId = row["Sort Name"].ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            //Active = row["active"].ToString() == "Y" ? true : false,
                        };
                        //check if server does not exist
                        if (!string.IsNullOrWhiteSpace(row["PaySourceId"].ToString()))
                        {
                            var existedPaySource = _paySourceService.GetByPaySourceId(paySource.PaySourceId);
                            if (existedPaySource == null)
                            {
                                addedPaySources.Add(paySource);
                            }
                            else
                            {
                                // ToDo: user automapper to automatically update model from viewmodel.
                                existedPaySource.Description = paySource.Description;
                                existedPaySource.Active = paySource.Active;
                                existedPaySource.ProgramId = paySource.ProgramId;

                                _paySourceService.UpdatePaySource(existedPaySource);
                            }
                        }
                    }
                    if (addedPaySources.Any())
                    {
                        _paySourceService.AddPaySources(addedPaySources);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }

        //

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