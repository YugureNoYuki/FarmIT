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
    public class BiologicalDisasterController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: BiologicalDisaster
        public ActionResult Index()
        {
            BiologicalDisaster bio = new BiologicalDisaster();
            return View(bio);
            //return View(db.BiologicalDisasters.ToList());
        }

        // GET: BiologicalDisaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BiologicalDisaster biologicalDisaster = db.BiologicalDisasters.Find(id);
            if (biologicalDisaster == null)
            {
                return HttpNotFound();
            }
            return View(biologicalDisaster);
        }

        // GET: BiologicalDisaster/Create
        public ActionResult Create()
        {
            BiologicalDisaster bio = new BiologicalDisaster();
            return View(bio);
        }

        // POST: BiologicalDisaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "BioDisasterID,BioDisasterDescr,BioDisasterNotes")]*/ BiologicalDisaster biologicalDisaster)
        {
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            var descrExist = IsDescrExist(biologicalDisaster.BioDisasterDescr);

            if (descrExist)
            {
                ModelState.AddModelError("NameExist", "Biological Disaster already exist");
                ViewBag.Error = "Biological Disaster already exists! Please specify different Name.";
                return View(biologicalDisaster);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.BiologicalDisasters.Add(biologicalDisaster);
                    db.SaveChanges();

                    biologicalDisaster.JavaScriptToRun = "mySuccess()";
                    return View(biologicalDisaster);
                }

                return View(biologicalDisaster);
            }

        }

        // GET: BiologicalDisaster/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BiologicalDisaster biologicalDisaster = db.BiologicalDisasters.Find(id);
            if (biologicalDisaster == null)
            {
                return HttpNotFound();
            }
            return View(biologicalDisaster);
        }

        // POST: BiologicalDisaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BioDisasterID,BioDisasterDescr,BioDisasterNotes")] BiologicalDisaster biologicalDisaster)
        {
            var IsExist = updExist(biologicalDisaster.BioDisasterDescr, biologicalDisaster.BioDisasterID);
            if (IsExist)
            {
                ModelState.AddModelError("BioDisaster", "Biological Disaster already exists. Please specify different Description.");
                ViewBag.Error = "Biological Disaster already exists. Please specify different Description.";
                return View(biologicalDisaster);
            }

            if (ModelState.IsValid)
            {
                db.Entry(biologicalDisaster).State = EntityState.Modified;
                db.SaveChanges();

                ////////////////////// ADDED IN CODE /////////////////////////
                biologicalDisaster.JavaScriptToRun = "mySuccess()";
                return View(biologicalDisaster);
            }
            return View(biologicalDisaster);
        }

        // GET: BiologicalDisaster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BiologicalDisaster biologicalDisaster = db.BiologicalDisasters.Find(id);
            if (biologicalDisaster == null)
            {
                return HttpNotFound();
            }

            //check if notnull and then remove
            if (biologicalDisaster != null)
            {
                try
                {
                    //remove supplier from db

                    db.BiologicalDisasters.Remove(biologicalDisaster);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    
                    biologicalDisaster.JavaScriptToRun = "mySuccess()";
                    return View("Index", biologicalDisaster);

                    //return RedirectToAction("Index");
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
            return View(biologicalDisaster);
        }

        // POST: BiologicalDisaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BiologicalDisaster biologicalDisaster = db.BiologicalDisasters.Find(id);
            db.BiologicalDisasters.Remove(biologicalDisaster);
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
        public bool IsDescrExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.BiologicalDisasters.Where(a => a.BioDisasterDescr == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool IsNameExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.BiologicalDisasters.Where(a => a.BioDisasterDescr == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.BiologicalDisasters.Where(a => a.BioDisasterID == inID).FirstOrDefault();


                var siloList = ctx.BiologicalDisasters.Where(a => a.BioDisasterID != inID).ToList();

                var outList = siloList.Where(a => a.BioDisasterDescr != myList.BioDisasterDescr);

                var match = outList.Where(a => a.BioDisasterDescr == inDescr).FirstOrDefault();

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
