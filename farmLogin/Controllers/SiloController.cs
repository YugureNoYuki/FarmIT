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
    public class SiloController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: Silo
        public ActionResult Index()
        {
            //var silos = db.Silos.Include(s => s.Unit);
            var silo = new Silo();
            return View(silo);
        }

        // GET: Silo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silo silo = db.Silos.Find(id);
            if (silo == null)
            {
                return HttpNotFound();
            }
            return View(silo);
        }

        // GET: Silo/Create
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr");
            Silo silo = new Silo();
            return View(silo);
        }

        // POST: Silo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "SiloID,SiloDescr,SiloCapacity,UnitID,SiloRentalFeePA,SiloStatus")]*/ Silo silo)
        {
            var descExist = IsDescExist(silo.SiloDescr);
            if (descExist == true)
            {
                ModelState.AddModelError("SiloExist", "Silo Description already in use. Please specify different Silo Description.");
                ViewBag.Error = "Silo Description already in use. Please specify different Silo Description.";

                ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
                return View(silo);
            }

            if (ModelState.IsValid)
            {
                silo.SiloStatus = "ACTIVE";
                db.Silos.Add(silo);
                db.SaveChanges();

                ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
                silo.JavaScriptToRun = "mySuccess()";
                return View(silo);
            }

            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
            return View(silo);
        }

        // GET: Silo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silo silo = db.Silos.Find(id);
            if (silo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
            return View(silo);
        }

        // POST: Silo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiloID,SiloDescr,SiloCapacity,UnitID,SiloRentalFeePA,SiloStatus")] Silo silo)
        {
            var IsExist = updExist(silo.SiloDescr, silo.SiloID);
            if (IsExist)
            {
                ModelState.AddModelError("SiloExist", "Silo Description already in use. Please specify different Silo Description.");
                ViewBag.Error = "Silo Description already in use. Please specify different Silo Description.";
                ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
                return View(silo);
            }

            if (ModelState.IsValid)
            {
                db.Entry(silo).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
                silo.JavaScriptToRun = "mySuccess()";
                return View(silo);
            }
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", silo.UnitID);
            return View(silo);
        }

        // GET: Silo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silo silo = db.Silos.Find(id);
            if (silo == null)
            {
                return HttpNotFound();
            }
            if (silo != null && silo.SiloHarvests.Count < 1)
            {
                db.Silos.Remove(silo);
                db.SaveChanges();
                silo.JavaScriptToRun = "myDelSuccess()";
                return View("Index", silo);
            }
            else
            {
                silo.JavaScriptToRun = "myFail()";
                return View("Index", silo);

            }
        }

        // POST: Silo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Silo silo = db.Silos.Find(id);
            db.Silos.Remove(silo);
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

        [NonAction]
        public bool IsDescExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Silos.Where(a => a.SiloDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.Silos.Where(a => a.SiloID == inID).FirstOrDefault();
                

                var siloList = ctx.Silos.Where(a => a.SiloID != inID).ToList();

                var outList = siloList.Where(a => a.SiloDescr != myList.SiloDescr);

                var match = outList.Where(a => a.SiloDescr == inDescr).FirstOrDefault();

                if (match != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
