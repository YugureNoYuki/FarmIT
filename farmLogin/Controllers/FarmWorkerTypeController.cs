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
    public class FarmWorkerTypeController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: FarmWorkerType
        public ActionResult Index()
        {
            FarmWorkerType fwtype = new FarmWorkerType();


            if (TempData["yay"] != null)
            {
                fwtype = (FarmWorkerType)TempData["yay"];
                TempData.Remove("yay");
            }


            return View(fwtype);
        }

        // GET: FarmWorkerType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorkerType farmWorkerType = db.FarmWorkerTypes.Find(id);
            if (farmWorkerType == null)
            {
                return HttpNotFound();
            }
            return View(farmWorkerType);
        }

        // GET: FarmWorkerType/Create
        public ActionResult Create()
        {

            FarmWorkerType fw = new FarmWorkerType();

            if (TempData["yay"] != null)
            {
                fw = (FarmWorkerType)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(fw);
        }

        // POST: FarmWorkerType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "FarmWorkerTypeID,FarmWorkerTypeDescr,FarmWorkerTypeNotes,FarmWorkerTypeActiveStatus")]*/ FarmWorkerType farmWorkerType)
        {

            var IsExist = IsDescExist(farmWorkerType.FarmWorkerTypeDescr);
            if (IsExist)
            {
                ModelState.AddModelError("FarmWorkerTypeExist", "Farm Worker Type already exist");
                ViewBag.Error = "Farm Worker Type already exists! Please specify different Description.";
                return View(farmWorkerType);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //db.InventoryTypes.Add(farmworertyper);

                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        var entity = new FarmWorkerType();

                        entity.FarmWorkerTypeID = farmWorkerType.FarmWorkerTypeID;
                        entity.FarmWorkerTypeDescr = farmWorkerType.FarmWorkerTypeDescr;
                        entity.FarmWorkerTypeNotes = farmWorkerType.FarmWorkerTypeNotes;
                        entity.FarmWorkerTypeActiveStatus = "ACTIVE";

                        //db.FarmWorkerTypes.Add(entity);

                        db.FarmWorkerTypes.Add(entity);

                        db.SaveChanges();
                        //return RedirectToAction("Index");
                        ViewBag.Success = "Success!";
                        //TempData["UserMessage"] = new MessageVM { CssClassName = "alert-success", Title = "Success!", Message = "Operation Done!" };
                        //return RedirectToAction("Create");


                        //return JavaScript("mySuccess();");
                        //return JavaScript("window.alert('HELLO');");

                        farmWorkerType.JavaScriptToRun = "mySuccess()";
                        return View(farmWorkerType);

                        //return View(inventoryType);
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
            }
            return View(farmWorkerType);
        }

        // GET: FarmWorkerType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorkerType farmWorkerType = db.FarmWorkerTypes.Find(id);
            if (farmWorkerType == null)
            {
                return HttpNotFound();
            }
            return View(farmWorkerType);
        }

        // POST: FarmWorkerType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FarmWorkerTypeID,FarmWorkerTypeDescr,FarmWorkerTypeNotes,FarmWorkerTypeActiveStatus")] FarmWorkerType farmWorkerType)
        {
            var IsExist = updExist(farmWorkerType.FarmWorkerTypeDescr);
            if (IsExist)
            {
                ModelState.AddModelError("FarmWorkerTypeExist", "Farm Worker Type already exist");
                ViewBag.Error = "Farm Worker Type already exists! Please specify different Description.";
                return View(farmWorkerType);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(farmWorkerType).State = EntityState.Modified;
                    db.SaveChanges();

                    if (TempData["yay"] != null)
                    {
                        farmWorkerType = (FarmWorkerType)TempData["yay"];
                        TempData.Remove("yay");
                    }

                    farmWorkerType.JavaScriptToRun = "mySuccess()";
                    return View(farmWorkerType);


                }
                return View(farmWorkerType);
            }
        }

        // GET: FarmWorkerType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorkerType farmWorkerType = db.FarmWorkerTypes.Find(id);
            if (farmWorkerType == null)
            {
                return HttpNotFound();
            }
            if (farmWorkerType.FarmWorkers.Count < 1 && farmWorkerType != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    //db.FarmWorkerTypes.Remove(farmWorkerType);
                    farmWorkerType.FarmWorkerTypeActiveStatus = "ARCHIVED";

                    db.SaveChanges();

                    var model = db.VehicleTypes.Select(e => e);
                    //var myModel = model.ToList();
                    //myModel.

                    //InventoryIndexViewModel myModel = new InventoryIndexViewModel();

                    farmWorkerType.JavaScriptToRun = "myDelSuccess()";
                    //TempData["success"] = "success";
                    return View("Index", farmWorkerType);

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
                ViewData["ErrorMessage"] = "Vehicle Type is in use by an Vehicle, cannot delete!";
                ViewBag.Error = "Vehicle Type is in use by an InventoryVehicle, cannot delete!";
                TempData["data"] = "Vehicle Type is in use by an Vehicle, cannot delete!";

                var model = db.FarmWorkerTypes.Select(e => e);

                farmWorkerType.JavaScriptToRun = "myFail()";
                return View("Index", farmWorkerType);

                //return RedirectToAction("Index");
            }
        }

        // POST: FarmWorkerType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FarmWorkerType farmWorkerType = db.FarmWorkerTypes.Find(id);
            db.FarmWorkerTypes.Remove(farmWorkerType);
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
                var v = ctx.FarmWorkerTypes.Where(a => a.FarmWorkerTypeDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist (string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.FarmWorkerTypes.Where(a => a.FarmWorkerTypeDescr != inDescr).ToList();

                var v = list.Where(a => a.FarmWorkerTypeDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }

    }
}
