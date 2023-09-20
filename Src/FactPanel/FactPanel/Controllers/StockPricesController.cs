using FactPanel.Business;
using FactPanel.Model;
using FactPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactPanel.Controllers
{
    public class StockPricesController : Controller
    {
        StockPricesService stockPricesService;

        public StockPricesController()
        {
            stockPricesService = new StockPricesService();
        }

        public ActionResult Filter()
        {
            return View();
        }


        // GET: StockPrices
        public ActionResult Index(StockPriceFilterViewModel filters)
        {
            var datas = stockPricesService.Get(filters.StockId,
                                            filters.FromDate,
                                            filters.ToDate,
                                            filters.page);


            ViewBag.Filters = filters;
            return View("Index", datas);
        }

        public ActionResult Details(int id = 0)
        {

            StockPrice stockPrice = stockPricesService.GetById(id);
            if (stockPrice == null)
            {
                return HttpNotFound();
            }
            return View(stockPrice);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockPrice stockPrice)
        {
            if (ModelState.IsValid)
            {
                stockPricesService.Add(stockPrice, new ModelStateWrapper(this.ModelState));

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");


            return View(stockPrice);
        }

        public ActionResult Edit(int id = 0)
        {

            StockPrice stockPrice = stockPricesService.GetById(id); ;
            if (stockPrice == null)
            {
                return HttpNotFound();
            }
            return View(stockPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockPrice stockPrice)
        {
            if (ModelState.IsValid)
            {
                stockPricesService.Edit(stockPrice, new ModelStateWrapper(this.ModelState));

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");

            return View(stockPrice);
        }

        public ActionResult Delete(int id)
        {

            StockPrice stockPrice = stockPricesService.GetById(id);
            if (stockPrice == null)
            {
                return HttpNotFound();
            }
            return View(stockPrice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            stockPricesService.Delete(id, new ModelStateWrapper(this.ModelState));


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