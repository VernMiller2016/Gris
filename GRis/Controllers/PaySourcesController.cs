﻿using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.Core.Utils;
using GRis.Extensions;
using GRis.ViewModels.Common;
using GRis.ViewModels.PaySource;
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
    public class PaySourcesController : BaseController
    {
        private IPaySourceService _paySourceService;

        public PaySourcesController(IPaySourceService paySourceService)
        {
            _paySourceService = paySourceService;
        }

        // GET: PaySources
        public ActionResult Index(PaysourceFilterViewModel filter, int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entites = _paySourceService.GetPaySources(pagingInfo, filter.PaysourceName);
            ViewBag.FilterViewModel = filter;
            var viewmodel = entites.ToMappedPagedList<PaySource, PaySourceDetailsViewModel>(pagingInfo);
            return View(viewmodel);
        }

        // GET: PaySources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _paySourceService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<PaySource, PaySourceDetailsViewModel>(entity);
            return View(viewmodel);
        }

        public ActionResult Create()
        {
            var viewmodel = new PaySourceAddViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaySourceAddViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                // check if vendor id already exists.
                if (_paySourceService.GetByVendorId(viewmodel.VendorId) == null)
                {
                    var entity = Mapper.Map<PaySourceAddViewModel, PaySource>(viewmodel);
                    _paySourceService.AddPaySource(entity);

                    Success($"<strong>{entity.Description}</strong> was successfully added.");
                    return RedirectToAction("Index");
                }
                else
                {
                    Danger($"A Paysource with same Id <strong>{viewmodel.VendorId}</strong> already exists.");
                }
            }

            return View(viewmodel);
        }

        // GET: PaySources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource entity = _paySourceService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }

            var viewmodel = Mapper.Map<PaySource, PaySourceEditViewModel>(entity);
            return View(viewmodel);
        }

        // POST: PaySources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaySourceEditViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                PaySource entity = _paySourceService.GetById(viewmodel.Id);
                if (entity == null)
                {
                    return HttpNotFound();
                }
                Mapper.Map(viewmodel, entity);

                _paySourceService.UpdatePaySource(entity);

                Success($"<strong>{entity.Description}</strong> was successfully updated.");
                return RedirectToAction("Index");
            }
            return View(viewmodel);
        }

        // GET: PaySources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource entity = _paySourceService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: PaySources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaySource entity = _paySourceService.GetById(id);
            if (entity != null) _paySourceService.Remove(entity);

            Success($"<strong>{entity.Description}</strong> was successfully deleted.");
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
                    int numOfPaySourcesUpdated = 0;
                    foreach (var row in dtServers.AsEnumerable().ToList())
                    {
                        var entityViewModel = new PaySourceAddViewModel()
                        {
                            VendorId = int.Parse(row["PaySourceId"].ToString()),
                            // some columns does not have ',' separater.
                            Description = row["Description"].ToString(),
                            //Active = row["active"].ToString() == "Y" ? true : false,
                        };
                        //check if paysource does not exist
                        if (!string.IsNullOrWhiteSpace(row["PaySourceId"].ToString()))
                        {
                            var existedEntity = _paySourceService.GetByVendorId(entityViewModel.VendorId);
                            if (existedEntity == null)
                            {
                                var entity = Mapper.Map<PaySourceAddViewModel, PaySource>(entityViewModel);
                                addedPaySources.Add(entity);
                            }
                            else
                            {
                                Mapper.Map(entityViewModel, existedEntity);
                                _paySourceService.UpdatePaySource(existedEntity);
                                numOfPaySourcesUpdated++;
                            }
                        }
                    }
                    if (addedPaySources.Any())
                    {
                        _paySourceService.AddPaySources(addedPaySources);
                    }
                    Success($"<strong>{addedPaySources.Count}</strong> PaySources have been successfully added. <br\\>"
                        + $"<strong>{numOfPaySourcesUpdated}</strong> PaySources have been successfully updated.");
                }
                return RedirectToAction("Index");
            }

            return View(viewmodel);
        }
    }
}