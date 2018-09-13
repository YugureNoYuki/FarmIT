﻿using System;
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
    public class TreatPlantationController : Controller
    {
        private FarmDbContext db = new FarmDbContext();
        public FieldTreatment ft = new FieldTreatment();

        // GET: TreatPlantation
        public ActionResult Index()
        {
            var inventoryTreatments = db.InventoryTreatments.Include(i => i.Inventory).Include(i => i.Treatment).Include(i => i.Unit1);
            return View(inventoryTreatments.ToList());
        }

        // GET: TreatPlantation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryTreatment inventoryTreatment = db.InventoryTreatments.Find(id);
            if (inventoryTreatment == null)
            {
                return HttpNotFound();
            }
            return View(inventoryTreatment);
        }

        // GET: TreatPlantation/Treat/1
        public ActionResult Treat(int? id) //plantationID
        {
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr");
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr");
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr");
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //GET PlantationID & FieldID
            using (FarmDbContext dc = new FarmDbContext())
            {
                var tplantation = dc.Plantations.Find(id); //return plantation id
                //if(tplantation == null)
                //{
                //    return HttpNotFound();
                //}
                //return View();
            }

            return View();
        }

        // POST: TreatPlantation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Treat([Bind(Include = "TreatmentID,InventoryID,TreatmentQnty,Unit")] InventoryTreatment inventoryTreatment, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.InventoryTreatments.Add(inventoryTreatment);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;
                }

                if (inventoryTreatment.TreatmentID == 2) //update field status when Id 2 = Stage 2 Treatment
                {
                    using (FarmDbContext dc = new FarmDbContext())
                    {
                        var tfield = (from a in dc.Plantations
                                      join b in dc.Fields on a.FieldID equals b.FieldID
                                      where a.PlantationID == id
                                      select a.FieldID).FirstOrDefault();
                        var f = dc.Fields.Find(tfield);
                        try
                        {
                            //remove 


                            f.FieldStatusID = 2; //Treated
                            //db.Entry(inventoryTreatment).State = EntityState.Modified;
                            dc.Entry(f).State = EntityState.Modified;
                            dc.SaveChanges();
                        }
                        catch (Exception err)
                        {

                            ViewBag.Message = err.Message;
                        }

                    }

                }
                using (FarmDbContext dc = new FarmDbContext())
                {
                    // inventoryTreatment.Inventory.InvQty -= inventoryTreatment.TreatmentQnty; //decrease inventory
                    var tfield1 = (from a in dc.Plantations                             //return the FieldID in Plantation
                                   join b in dc.Fields on a.FieldID equals b.FieldID
                                   where a.PlantationID == id
                                   select a.FieldID).FirstOrDefault();
                    ft.FieldID = Convert.ToInt32(tfield1);

                    //GET the TreatmentID on post
                    int treatId = inventoryTreatment.TreatmentID;
                    ft.TreatmentID = Convert.ToInt32(treatId);
                    ft.TreatmentDate = System.DateTime.Now;
                    try
                    {
                        db.FieldTreatments.Add(ft);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e.Message;
                    }

                }


                //CreateFieldTreatment(inventoryTreatment.TreatmentID)
                return RedirectToAction("Index");
            }

            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventoryTreatment.Unit);
            return View(inventoryTreatment);
        }

        // GET: TreatPlantation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryTreatment inventoryTreatment = db.InventoryTreatments.Find(id);
            if (inventoryTreatment == null)
            {
                return HttpNotFound();
            }
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventoryTreatment.Unit);
            return View(inventoryTreatment);
        }

        // POST: TreatPlantation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentID,InventoryID,TreatmentQnty,Unit")] InventoryTreatment inventoryTreatment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryTreatment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventoryTreatment.Unit);
            return View(inventoryTreatment);
        }

        // GET: TreatPlantation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryTreatment inventoryTreatment = db.InventoryTreatments.Find(id);
            if (inventoryTreatment == null)
            {
                return HttpNotFound();
            }
            return View(inventoryTreatment);
        }

        // POST: TreatPlantation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryTreatment inventoryTreatment = db.InventoryTreatments.Find(id);
            db.InventoryTreatments.Remove(inventoryTreatment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public void CreateFieldTreatment(int FieldId, int TreatmentId)
        {

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