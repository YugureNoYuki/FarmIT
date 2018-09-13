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
using farmLogin.ViewModels;

namespace farmLogin.Controllers
{
    public class InventoryTypeController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: InventoryType
        public ActionResult Index()
        {
            InventoryType invtype = new InventoryType();
            return View(invtype);
        }

        // GET: InventoryType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryType inventoryType = db.InventoryTypes.Find(id);
            if (inventoryType == null)
            {
                return HttpNotFound();
            }
            return View(inventoryType);
        }

        // GET: InventoryType/Create
        public ActionResult Create()
        {
            InventoryType inventoryType = new InventoryType();
            //inventoryType.JavaScriptToRun = "None";
            return View(inventoryType);
        }

        // POST: InventoryType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "InvTypeID,InvTypeDescr")]*/ InventoryType inventoryType)
        {
            var IsExist = IsDescExist(inventoryType.InvTypeDescr);
            if (IsExist)
            {
                ModelState.AddModelError("InvTypeExist", "Inventory Type already exist");
                ViewBag.Error = "Inventory Type already exists! Please specify different Description.";
                return View(inventoryType);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //db.InventoryTypes.Add(inventoryType);

                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        var entity = new InventoryType();

                        entity.InvTypeID = inventoryType.InvTypeID;
                        entity.InvTypeDescr = inventoryType.InvTypeDescr;

                        db.InventoryTypes.Add(entity);

                        db.SaveChanges();
                        //return RedirectToAction("Index");
                        ViewBag.Success = "Success!";
                        //TempData["UserMessage"] = new MessageVM { CssClassName = "alert-success", Title = "Success!", Message = "Operation Done!" };
                        //return RedirectToAction("Create");


                        //return JavaScript("mySuccess();");
                        //return JavaScript("window.alert('HELLO');");

                        inventoryType.JavaScriptToRun = "mySuccess()";
                        return View(inventoryType);

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


               //db.SaveChanges();
               /* return RedirectToAction("Index")*/;
                }
            }

            return View(inventoryType);
        }

        // GET: InventoryType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryType inventoryType = db.InventoryTypes.Find(id);
            if (inventoryType == null)
            {
                return HttpNotFound();
            }
            return View(inventoryType);
        }

        // POST: InventoryType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvTypeID,InvTypeDescr")] InventoryType inventoryType)
        {
            var IsExist = IsDescExist(inventoryType.InvTypeDescr);
            if (IsExist)
            {
                ModelState.AddModelError("InvTypeExist", "Inventory Type already exist");
                ViewBag.Error = "Inventory Type already exists! Please specify different Description.";
                return View(inventoryType);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(inventoryType).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");

                    ////////////////////// ADDED IN CODE /////////////////////////
                    inventoryType.JavaScriptToRun = "mySuccess()";
                    return View(inventoryType);


                }
                return View(inventoryType);
            }

        }

        //GET: InventoryType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryType inventoryType = db.InventoryTypes.Find(id);
            if (inventoryType == null)
            {
                return HttpNotFound();
            }
            //return View(inventoryType);

            if (inventoryType.Inventories.Count < 1 && inventoryType != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.InventoryTypes.Remove(inventoryType);
                    db.SaveChanges();

                    var model = db.InventoryTypes.Select(e => e);
                    //var myModel = model.ToList();
                    //myModel.

                    //InventoryIndexViewModel myModel = new InventoryIndexViewModel();

                    inventoryType.JavaScriptToRun = "mySuccess()";
                    //TempData["success"] = "success";
                    return View("Index", inventoryType);
                    
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
                ViewData["ErrorMessage"] = "Inventory Type is in use by an Inventory Item, cannot delete!";
                ViewBag.Error = "Inventory Type is in use by an Inventory Object, cannot delete!";
                TempData["data"] = "Inventory Type is in use by an Inventory Object, cannot delete!";

                var model = db.InventoryTypes.Select(e => e);

                inventoryType.JavaScriptToRun = "myFail()";
                return View("Index", inventoryType);
                
                //return RedirectToAction("Index");
            }

            //RedirectToAction("Index");

            //return RedirectToAction("DeleteConfirmed", "InventoryType");
        }

        //GET: InventoryType/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    return RedirectToAction("DeleteConfirmed", "InventoryType");
        //}



        // POST: InventoryType/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryType inventoryType = db.InventoryTypes.Find(id);
            //var model = db.InventoryTypes.Where(x => x.InvTypeID == id).FirstOrDefault();
            
            //if (model != null && model.Inventories.Count < 1)
            //{
            //    try
            //    {
                    
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //}

            if (inventoryType.Inventories.Count < 1 && inventoryType != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.InventoryTypes.Remove(inventoryType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                ViewBag.Error = "Inventory Type is in use by an Inventory Object, cannot delete!";
                return RedirectToAction("Index", "InventoryType");
                //return View(inventoryType);
            }
            //db.InventoryTypes.Remove(inventoryType);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
                var v = ctx.InventoryTypes.Where(a => a.InvTypeDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.InventoryTypes.Where(a => a.InvTypeID == inID).FirstOrDefault();


                var excList = ctx.InventoryTypes.Where(a => a.InvTypeID != inID).ToList();

                var outList = excList.Where(a => a.InvTypeDescr != myList.InvTypeDescr);

                var match = outList.Where(a => a.InvTypeDescr == inDescr).FirstOrDefault();

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
