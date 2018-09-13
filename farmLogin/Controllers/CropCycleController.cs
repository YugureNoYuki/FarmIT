using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using farmLogin.ViewModels;
using PagedList;
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class CropCycleController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: CropCycle
        public ActionResult Index(int? page, string search)
        {
            CropCycleIndexViewModel viewModel = new CropCycleIndexViewModel();
            var cropcycles = db.CropCycles.Include(c => c.Plantations);
            if (!String.IsNullOrEmpty(search))
            {
                cropcycles = db.CropCycles.Where(c => c.CropCycleDescr.Contains(search));
                //ViewBag.Search = search;
                viewModel.Search = search;
            }
            //sort results
            cropcycles = cropcycles.OrderBy(c => c.CropCycleDescr);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.CropCycles = cropcycles.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: CropCycle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropCycle cropCycle = db.CropCycles.Find(id);
            if (cropCycle == null)
            {
                return HttpNotFound();
            }
            return View(cropCycle);
        }

        // GET: CropCycle/Create
        public ActionResult Create()
        {
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            CropCycle cropcycle = new CropCycle();

            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            return View(cropcycle);
        }

        // POST: CropCycle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "CropCycleID,CropCycleStartDate,CropCycleEndDate,CropCycleDescr")]*/ CropCycle cropCycle)
        {
            var nameExist = IsNameExist(cropCycle.CropCycleDescr);
            if (nameExist)
            {
                ModelState.AddModelError("CropCycleExist", "Crop Cycle already exists, please specify different Description.");
                ViewBag.Error = "Crop Cycle already exists, please specify different Description.";
                return View(cropCycle);
            }

            if (ModelState.IsValid)
            {
                db.CropCycles.Add(cropCycle);
                db.SaveChanges();
                cropCycle.JavaScriptToRun = "mySuccess()";
                return View(cropCycle);
            }

            return View(cropCycle);
        }

        // GET: CropCycle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropCycle cropCycle = db.CropCycles.Find(id);
            if (cropCycle == null)
            {
                return HttpNotFound();
            }

            //cropCycle.JavaScriptToRun = "mySuccess()";
            return View(cropCycle);
        }

        // POST: CropCycle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CropCycleID,CropCycleStartDate,CropCycleEndDate,CropCycleDescr")] CropCycle cropCycle)
        {
            var IsExist = updExist(cropCycle.CropCycleDescr, cropCycle.CropCycleID);
            if (IsExist)
            {
                ModelState.AddModelError("CycleExist", "Crop Cycle already exists.");
                ViewBag.Error = "Crop cycle already exists. Please specify different Description.";
                return View(cropCycle);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cropCycle).State = EntityState.Modified;
                db.SaveChanges();
                cropCycle.JavaScriptToRun = "mySuccess()";
                return View(cropCycle);
            }
            return View(cropCycle);
        }

        // GET: CropCycle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropCycle cropCycle = db.CropCycles.Find(id);
            if (cropCycle == null)
            {
                return HttpNotFound();
            }
            //check if notnull and then remove
            if (cropCycle != null)
            {
                try
                {
                    //remove supplier from db

                    db.CropCycles.Remove(cropCycle);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    CropCycleIndexViewModel viewModel = new CropCycleIndexViewModel();
                    var cropcycles = db.CropCycles.Include(c => c.Plantations);
                    if (!String.IsNullOrEmpty(""))
                    {
                        cropcycles = db.CropCycles.Where(c => c.CropCycleDescr.Contains(""));
                        //ViewBag.Search = search;
                        viewModel.Search = "";
                    }
                    //sort results
                    cropcycles = cropcycles.OrderBy(c => c.CropCycleDescr);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.CropCycles = cropcycles.ToPagedList(currentPage, PageItems);
                    return View(viewModel);
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

            return View(cropCycle);
        }

        // POST: CropCycle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CropCycle cropCycle = db.CropCycles.Find(id);
            db.CropCycles.Remove(cropCycle);
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
                var v = ctx.CropCycles.Where(a => a.CropCycleDescr == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.CropCycles.Where(a => a.CropCycleID == inID).FirstOrDefault();


                var siloList = ctx.CropCycles.Where(a => a.CropCycleID != inID).ToList();

                var outList = siloList.Where(a => a.CropCycleDescr != myList.CropCycleDescr);

                var match = outList.Where(a => a.CropCycleDescr == inDescr).FirstOrDefault();

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
