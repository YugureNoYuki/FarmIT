using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;

namespace farmLogin.Controllers
{
    public class HarvestSaleController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: HarvestSale
        public ActionResult Index()
        {
            var sales = db.Sales.Include(s => s.Customer).Include(s => s.Unit);
            return View(sales.ToList());
        }

        // GET: HarvestSale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: HarvestSale/Create
        public ActionResult Create(int? id) //SiloID
        {
            TempData["siloId"] = id;
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerFName");
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr");
            return View();
        }

        // POST: HarvestSale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleID,SaleDate,SaleQty,UnitID,SaleAmnt,CustomerID,PurchaseAgreement")] Sale sale) //here we need to the silo id so we know which silo to reduce our stock from
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();

                //Push this SaleId + SaleQty + SaleAmnt to SiloHarvest Sale table
                using (FarmDbContext dc = new FarmDbContext())
                {
                    SiloHarvestSale shs = new SiloHarvestSale();
                    shs.SaleID = sale.SaleID;
                    shs.SiloHarvestSaleTotalQty = sale.SaleQty;
                    shs.SiloHarvestSaleTotalAmnt = sale.SaleAmnt;
                    int siloH = (int)TempData["siloId"];
                    //var saleSilo = dc.Silos.Find(siloId); //check if the siloId exists for which we are capturing the 
                    var saleSilo = dc.Silos.Find(TempData["siloId"]);
                    //var shId = (from a in dc.SiloHarvests
                    //            join b in dc.Silos on a.SiloID equals b.SiloID
                    //            where b.SiloID == silo
                    //            //group a 
                    //            select a.SiloHarvestID);
                    shs.SiloHarvestID = siloH;
                    dc.SiloHarvestSales.Add(shs); //populating complete
                    dc.SaveChanges();

                    //reduce harvest tonnes stored
                    var siloHV = dc.SiloHarvests.Find(siloH);

                    //check if Harvest tonnes stored not empty and will be enough for sale
                    if (siloHV.SiloHarvestTonnesStored > sale.SaleAmnt)
                    {
                        siloHV.SiloHarvestTonnesStored -= sale.SaleAmnt;
                        //db.Entry(inventoryTreatment).State = EntityState.Modified;
                        dc.Entry(siloHV).State = EntityState.Modified;
                        dc.SaveChanges();
                    }
                    else //return error
                    {
                        ViewBag.Message = "There is not enough harvest available to complete the sale";
                    }


                }

                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", sale.CustomerID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", sale.UnitID);
            return View(sale);
        }

        // GET: HarvestSale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", sale.CustomerID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", sale.UnitID);
            return View(sale);
        }

        // POST: HarvestSale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,SaleDate,SaleQty,UnitID,SaleAmnt,CustomerID,PurchaseAgreement")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerFName", sale.CustomerID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", sale.UnitID);
            return View(sale);
        }

        // GET: HarvestSale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: HarvestSale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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
