using FactPanel.Business;
using FactPanel.Model;
using FactPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactPanel.Controllers
{
    public class FactRunsController : Controller
    {
        FactRunsService factRunsService;

        public FactRunsController()
        {
            factRunsService = new FactRunsService();

        }

        // GET: FactRuns/Filter
        [Authorize(Roles = "MarketView,Market,Admin")]
        public ActionResult Filter()
        {

            return View();
        }

        // GET: FactRuns
        [Authorize(Roles = "MarketView,Market,Admin")]
        public ActionResult Index(FactRunFilterViewModel filters)
        {
            var factRuns = factRunsService.Get(filters.FromDate,
                                                    filters.ToDate,
                                                    filters.UploadSuccess,
                                                    filters.page);
            ViewBag.Filters = filters;
            return View("Index", factRuns);
        }

        // GET: FactRuns/Details/5
        [Authorize(Roles = "MarketView,Market,Admin")]
        public ActionResult Details(int id = 0)
        {

            FactRun factRun = factRunsService.GetById(id);
            if (factRun == null)
            {
                return HttpNotFound();
            }
            return View(factRun);
        }

        // GET: FactRuns/Create
        [Authorize(Roles = "Market,Admin")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: FactRuns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Market,Admin")]
        public ActionResult Create(FactRun factRun)
        {
           
            if (ModelState.IsValid)
            {
                factRunsService.Add(factRun, new ModelStateWrapper(this.ModelState));

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");



            return View(factRun);
        }

        // GET: FactRuns/Edit/5
        [Authorize(Roles = "Market,Admin")]
        public ActionResult Edit(int id = 0)
        {

            FactRun factRun = factRunsService.GetById(id);
            if (factRun == null)
            {
                return HttpNotFound();
            }

            return View(factRun);
        }

        // POST: FactRuns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Market,Admin")]
        public ActionResult Edit(FactRun factRun)
        {
            if (ModelState.IsValid)
            {
                factRunsService.Edit(factRun, new ModelStateWrapper(this.ModelState));

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");


            return View(factRun);
        }

        // GET: FactRuns/Delete/5
        [Authorize(Roles = "Market,Admin")]
        public ActionResult Delete(int id)
        {

            FactRun factRun = factRunsService.GetById(id);
            if (factRun == null)
            {
                return HttpNotFound();
            }
            return View(factRun);
        }

        // POST: FactRuns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Market,Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            factRunsService.Delete(id, new ModelStateWrapper(this.ModelState));


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}