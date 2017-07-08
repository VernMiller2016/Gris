using AutoMapper;
using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using GRis.Extensions;
using GRis.ViewModels.Element;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class ElementsController : BaseController
    {
        private readonly IElementService _elementService;

        public ElementsController(IElementService elementService)
        {
            _elementService = elementService;
        }

        public ActionResult Index(int page = 1)
        {
            var pagingInfo = new PagingInfo() { PageNumber = page };
            var entites = _elementService.GetElements(pagingInfo);
            var viewmodel = entites.ToMappedPagedList<Element, ElementDetailsViewModel>(pagingInfo);
            return View(viewmodel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _elementService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Element, ElementDetailsViewModel>(entity);
            return View(viewmodel);
        }

        public ActionResult Create()
        {
            var viewmodel = new ElementAddViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Create(ElementAddViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<ElementAddViewModel, Element>(viewmodel);
                    _elementService.AddElement(entity);

                    Success($"<strong>{entity.DisplayName}</strong> was successfully added.");
                    return RedirectToAction("Index");
                }
                return View(viewmodel);
            }
            catch
            {
                return View(viewmodel);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = _elementService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Element, ElementEditViewModel>(entity);
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Edit(int id, ElementEditViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = _elementService.GetById(viewmodel.Id);
                    if (entity == null)
                    {
                        return HttpNotFound();
                    }
                    Mapper.Map(viewmodel, entity);
                    _elementService.UpdateElement(entity);

                    Success($"<strong>{entity.DisplayName}</strong> was successfully updated.");
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
            var entity = _elementService.GetById(id.Value);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var viewmodel = Mapper.Map<Element, ElementDetailsViewModel>(entity);
            return View(viewmodel);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _elementService.GetById(id);
                if (entity == null)
                {
                    return HttpNotFound();
                }

                _elementService.Remove(entity);

                Success($"<strong>{entity.DisplayName}</strong> was successfully deleted.");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}