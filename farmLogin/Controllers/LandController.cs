using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class LandController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: Land
        public ActionResult Index()
        {
            var lands = new Land();
            return View(lands);
        }

        // GET: Land/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Land land = db.Lands.Find(id);
            if (land == null)
            {
                return HttpNotFound();
            }
            return View(land);
        }

        // GET: Land/Create
        public ActionResult Create()
        {
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName");
            Land land = new Land();
            return View(land);
        }

        // POST: Land/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LandID,LandName,FarmID")] Land land)
        {
            var nameExist = IsNameExist(land.LandName);
            if (nameExist)
            {
                ModelState.AddModelError("LandExist", "Land already exist, please specify different Land Name.");
                ViewBag.Error = "Land already exists! Please specify different Land Name.";
                ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
                return View(land);
            }
            if (ModelState.IsValid)
            {
                db.Lands.Add(land);
                db.SaveChanges();
                land.JavaScriptToRun = "mySuccess()";
                ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
                return View(land);
            }

            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
            return View(land);
        }

        // GET: Land/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Land land = db.Lands.Find(id);
            if (land == null)
            {
                return HttpNotFound();
            }
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
            return View(land);
        }

        // POST: Land/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LandID,LandName,FarmID")] Land land)
        {
            var IsExist = updExist(land.LandName);
            if (IsExist)
            {
                ModelState.AddModelError("LandExist", "Land already exists, please specify different Land Name.");
                ViewBag.Error = "Land already exists! Please specify different Land Name.";
                return View(land);
            }

            if (ModelState.IsValid)
            {
                db.Entry(land).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
                land.JavaScriptToRun = "mySuccess()";
                return View(land);
            }
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", land.FarmID);
            return View(land);
        }

        // GET: Land/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Land land = db.Lands.Find(id);
            if (land == null)
            {
                return HttpNotFound();
            }
            if (land.Fields.Count < 1 && land != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.Lands.Remove(land);

                    db.SaveChanges();

                    land.JavaScriptToRun = "myDelSuccess()";
                    // return View("Index", land);

                    var farms = new Farm();
                    farms.JavaScriptToRun = "myDelSuccess()";
                    return View("../Farm/Index", farms);

                    //return RedirectToAction("Index", inventoryType);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            else
            {
                Farm farm = new Farm();
                ViewData["ErrorMessage"] = "Land cannot be deleted! Fields are linked to it.";
                ViewBag.Error = "Land cannot be deleted! Fields are linked to it.";
                TempData["data"] = "Land cannot be deleted! Fields are linked to it.";

                land.JavaScriptToRun = "myLandFail()";
                farm.JavaScriptToRun = "myLandFail()";
                //return View("Index", land);
                //return View("Index", farm);


                //return RedirectToAction("Index", "Farm", farm);
                return View("../Farm/Index", farm);

                //return RedirectToAction("Index");
            }
        }

        // POST: Land/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Land land = db.Lands.Find(id);
            db.Lands.Remove(land);
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
        public bool IsNameExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Lands.Where(a => a.LandName == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.Lands.Where(a => a.LandName != inDescr).ToList();

                var v = list.Where(a => a.LandName == inDescr).FirstOrDefault();
                return v != null;
            }
        }
    }
}
