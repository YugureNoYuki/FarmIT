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
    public class FarmController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Farm
        public ActionResult Index()
        {
            //var farms = db.Farms.Include(f => f.Province);
            var farms = new Farm();
            return View(farms);
        }

        // GET: Farm/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farm farm = db.Farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        // GET: Farm/Create
        public ActionResult Create()
        {
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");
            Farm farm = new Farm();

            return View(farm);
        }

        // POST: Farm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "FarmID,FarmName,ProvinceID")]*/ Farm farm)
        {
            var nameExist = IsNameExist(farm.FarmName);
            if (nameExist)
            {
                ModelState.AddModelError("FarmExist", "Farm already exist");
                ViewBag.Error = "Farm already exists! Please specify different Farm Name.";
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");
                return View(farm);
            }
            if (ModelState.IsValid)
            {
                db.Farms.Add(farm);
                db.SaveChanges();
                farm.JavaScriptToRun = "mySuccess()";
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");
                return View(farm);
            }

            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farm.ProvinceID);
            return View(farm);
        }

        // GET: Farm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farm farm = db.Farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farm.ProvinceID);
            return View(farm);
        }

        // POST: Farm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FarmID,FarmName,ProvinceID")] Farm farm)
        {
            var IsExist = updExist(farm.FarmName);
            if (IsExist)
            {
                ModelState.AddModelError("FarmWorkerTypeExist", "Farm Worker Type already exist");
                ViewBag.Error = "Farm Worker Type already exists! Please specify different Description.";
                return View(farm);
            }

            if (ModelState.IsValid)
            {
                db.Entry(farm).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farm.ProvinceID);
                farm.JavaScriptToRun = "mySuccess()";
                return View(farm);
            }
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farm.ProvinceID);
            return View(farm);
        }

        // GET: Farm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farm farm = db.Farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            if (farm.Lands.Count < 1 && farm != null)
            {
                try
                {
                    db.Farms.Remove(farm);

                    db.SaveChanges();

                    farm.JavaScriptToRun = "myDelSuccess()";
                    return View("Index", farm);

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
                ViewData["ErrorMessage"] = "Farm cannot be deleted! Lands are linked to it.";
                ViewBag.Error = "Farm cannot be deleted! Lands are linked to it.";
                TempData["data"] = "Farm cannot be deleted! Lands are linked to it.";

                var model = db.FarmWorkerTypes.Select(e => e);

                farm.JavaScriptToRun = "myFail()";
                return View("Index", farm);

                //return RedirectToAction("Index");
            }
        }

        // POST: Farm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Farm farm = db.Farms.Find(id);
            db.Farms.Remove(farm);
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
                var v = ctx.Farms.Where(a => a.FarmName == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.Farms.Where(a => a.FarmName != inDescr).ToList();

                var v = list.Where(a => a.FarmName == inDescr).FirstOrDefault();
                return v != null;
            }
        }
    }
}
