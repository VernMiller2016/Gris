﻿using AutoMapper;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.ViewModels.Program;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ProgramsController : Controller
    {
        private IProgramService _programService;
        private IPaySourceService _paySourceService;

        public ProgramsController(IProgramService programService, IPaySourceService paySourceService)
        {
            this._programService = programService;
            this._paySourceService = paySourceService;
        }

        // GET: Programs
        public ActionResult Index()
        {
            var viewmodel = Mapper.Map<IEnumerable<Program>, IEnumerable<ProgramDetailsViewModel>>(_programService.GetPrograms().ToList());
            return View(viewmodel);
        }

        // GET: Programs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program entity = _programService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Program, ProgramDetailsViewModel>(entity);
            return View(viewmodel);
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            var viewmodel = new ProgramAddViewModel();
            viewmodel.PaySources = _programService.GetAvailablePaySourcesNotRelatedToPrograms().Select(t => new SelectListItem()
            {
                Text = t.VendorId.ToString(),
                Value = t.Id.ToString()
            });
            return View(viewmodel);
        }

        // POST: Programs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProgramAddViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<ProgramAddViewModel, Program>(viewmodel);
                    var selectedPaysourcesList = viewmodel.SelectedPaySources.Select(t => new PaySource() { Id = t }).ToList();
                    foreach (var id in viewmodel.SelectedPaySources.AsNotNull())
                    {
                        var paysource = _paySourceService.GetById(id);
                        entity.PaySources.Add(paysource);
                    }
                    _programService.AddProgram(entity);
                    return RedirectToAction("Index");
                }
                viewmodel.PaySources = _programService.GetAvailablePaySourcesNotRelatedToPrograms().Select(t => new SelectListItem()
                {
                    Text = t.VendorId.ToString(),
                    Value = t.Id.ToString()
                });
                return View(viewmodel);
            }
            catch
            {
                viewmodel.PaySources = _programService.GetAvailablePaySourcesNotRelatedToPrograms().Select(t => new SelectListItem()
                {
                    Text = t.VendorId.ToString(),
                    Value = t.Id.ToString()
                });
                return View(viewmodel);
            }
        }

        // GET: Programs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program entity = _programService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Program, ProgramEditViewModel>(entity);
            return View(viewmodel);
        }

        // POST: Programs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProgramEditViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Program entity = _programService.GetById(viewmodel.Id);
                    if (entity == null)
                    {
                        return HttpNotFound();
                    }
                    Mapper.Map(viewmodel, entity);

                    _programService.UpdateProgram(entity);
                    return RedirectToAction("Index");
                }
                return View(viewmodel);
            }
            catch
            {
                return View(viewmodel);
            }
        }

        // GET: Programs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program entity = _programService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Program entity = _programService.GetById(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }

                _programService.Remove(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}