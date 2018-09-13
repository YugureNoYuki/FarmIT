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
    public class CropTypeController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: CropType
        public ActionResult Index()
        {
            var croptype = new CropType();
            return View(croptype);
        }

        // GET: CropType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropType cropType = db.CropTypes.Find(id);
            if (cropType == null)
            {
                return HttpNotFound();
            }
            return View(cropType);
        }

        // GET: CropType/Create
        public ActionResult Create()
        {
            var croptype = new CropType();
            return View(croptype);
        }

        // POST: CropType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CropTypeID,CropTypeDescr,MaturityDays")] CropType cropType)
        {
            var nameExist = IsNameExist(cropType.CropTypeDescr);
            if (nameExist)
            {
                ModelState.AddModelError("CropTypeExist", "Crop Type already exist, please specify different Crop Type!");
                ViewBag.Error = "Crop Type already exist, please specify different Crop Type!";
                return View(cropType);
            }
            if (ModelState.IsValid)
            {
                db.CropTypes.Add(cropType);
                db.SaveChanges();
                cropType.JavaScriptToRun = "mySuccess()";
                return View(cropType);
            }

            return View(cropType);
        }

        // GET: CropType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropType cropType = db.CropTypes.Find(id);
            if (cropType == null)
            {
                return HttpNotFound();
            }
            return View(cropType);
        }

        // POST: CropType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CropTypeID,CropTypeDescr,MaturityDays")] CropType cropType)
        {
            var IsExist = updExist(cropType.CropTypeDescr);
            if (IsExist)
            {
                ModelState.AddModelError("CropTypeExist", "Crop Type already exist, please specify different Crop Type!");
                ViewBag.Error = "Crop Type already exist, please specify different Crop Type!";
                return View(cropType);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cropType).State = EntityState.Modified;
                db.SaveChanges();
                cropType.JavaScriptToRun = "mySuccess()";
                return View(cropType);
            }
            return View(cropType);
        }

        // GET: CropType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropType cropType = db.CropTypes.Find(id);
            if (cropType == null)
            {
                return HttpNotFound();
            }
            if (cropType.Plantations.Count < 1 && cropType != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.CropTypes.Remove(cropType);

                    db.SaveChanges();

                    cropType.JavaScriptToRun = "myDelSuccess()";
                    return View("Index", cropType);

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
                ViewData["ErrorMessage"] = "Crop Type cannot be deleted! Plantations are linked to it.";
                ViewBag.Error = "Crop Type cannot be deleted! Plantations are linked to it.";
                TempData["data"] = "Crop Type cannot be deleted! Plantations are linked to it.";

                cropType.JavaScriptToRun = "myFail()";
                return View("Index", cropType);

                //return RedirectToAction("Index");
            }
        }

        // POST: CropType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CropType cropType = db.CropTypes.Find(id);
            db.CropTypes.Remove(cropType);
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
                var v = ctx.CropTypes.Where(a => a.CropTypeDescr == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.CropTypes.Where(a => a.CropTypeDescr != inDescr).ToList();

                var v = list.Where(a => a.CropTypeDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }
    }
}

