using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Core.Extensions;
using GRis.ViewModels.Program;
using System;
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
            var programs = _programService.GetPrograms();
            return View(programs);
        }

        // GET: Programs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = _programService.GetById(id.Value);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            var viewmodel = new ProgramAddViewModel();
            viewmodel.PaySources = _programService.GetAvailablePaySourcesNotRelatedToPrograms().Select(t => new SelectListItem()
            {
                Text = t.PaySourceId.ToString(),
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
                    var selectedPaysourcesList = viewmodel.SelectedPaySources.Select(t => new PaySource() { Id = t }).ToList();
                    var entity = new Program()
                    {
                        Description = viewmodel.Description,
                        Name = viewmodel.Name,
                        GpProject = viewmodel.GpProject,
                        Active = viewmodel.Active,
                        PaySources = new List<PaySource>()
                    };
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
                    Text = t.PaySourceId.ToString(),
                    Value = t.Id.ToString()
                });
                return View(viewmodel);
            }
            catch
            {
                viewmodel.PaySources = _programService.GetAvailablePaySourcesNotRelatedToPrograms().Select(t => new SelectListItem()
                {
                    Text = t.PaySourceId.ToString(),
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
            Program program = _programService.GetById(id.Value);
            if (program == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new ProgramEditViewModel()
            {
                Id = program.Id,
                Name = program.Name,
                Description = program.Description,
                GpProject = program.GpProject,
                Active = program.Active
            };
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
                    Program existedEntity = _programService.GetById(viewmodel.Id);
                    if (existedEntity == null)
                    {
                        return HttpNotFound();
                    }
                    // ToDo: user automapper to automatically update model from viewmodel.
                    existedEntity.Name = viewmodel.Name;
                    existedEntity.Description = viewmodel.Description;
                    existedEntity.GpProject = viewmodel.GpProject;
                    existedEntity.Active = viewmodel.Active;

                    _programService.UpdateProgram(existedEntity);
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