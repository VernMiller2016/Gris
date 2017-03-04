using GRis.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GRis.Controllers
{
    public class PaySourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaySources
        public ActionResult Index()
        {
            var paySources = db.PaySources.Include(p => p.Program);
            return View(paySources.ToList());
        }

        // GET: PaySources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource paySource = db.PaySources.Find(id);
            if (paySource == null)
            {
                return HttpNotFound();
            }
            return View(paySource);
        }

        // GET: PaySources/Create
        public ActionResult Create()
        {
            ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description");
            return View();
        }

        // POST: PaySources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaySourceId,Description,Active,ProgramId")] PaySource paySource)
        {
            if (ModelState.IsValid)
            {
                db.PaySources.Add(paySource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // GET: PaySources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource paySource = db.PaySources.Find(id);
            if (paySource == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // POST: PaySources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaySourceId,Description,Active,ProgramId")] PaySource paySource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paySource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "ProgramId", "Description", paySource.ProgramId);
            return View(paySource);
        }

        // GET: PaySources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaySource paySource = db.PaySources.Find(id);
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
            PaySource paySource = db.PaySources.Find(id);
            if (paySource != null) db.PaySources.Remove(paySource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
