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
using System.Web.Script.Serialization;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace farmLogin.Controllers
{
    public class PlantationController : Controller
    {
        private FarmDbContext db = new FarmDbContext();
        private int routeId;

        #region CRUD and ActionPage
        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Plantation

        public ActionResult Index()
        {
            //var plantation = db.Plantations.Include(p => p.CropCycle).Include(p => p.CropType).Include(p => p.Field).Include(p => p.SiloHarvests).Where(p => p.PlantationStatus == "Planned");
            return View();
        }

        public ActionResult Plantations()
        {
            //var plantation = db.Plantations.Include(p => p.CropCycle).Include(p => p.CropType).Include(p => p.Field).Include(p => p.SiloHarvests).Where(p => p.PlantationStatus == "Confirmed");
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantation plantation = db.Plantations.Find(id);
            if (plantation == null)
            {
                return HttpNotFound();
            }
            return View(plantation);
        }

        // GET: Plantation/Create
        public ActionResult Create()
        {
            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr");
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr");
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName");
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr");
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr");
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr");

            Plantation plantation = new Plantation();

            return View(plantation);
        }

        // POST: Plantation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Plantation plantation)
        {
            plantation.CropCycleID = 1;
            plantation.FieldStageID = 1;
            plantation.PlantationStatus = "Planned";

            if (ModelState.IsValid)
            {
                plantation.CropCycleID = 1; //This is not correct
                plantation.FieldStageID = 1;
                db.Plantations.Add(plantation);
                db.SaveChanges();
                ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
                ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
                ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
                ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
                ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
                ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
                plantation.JavaScriptToRun = "mySuccess()";
                return View(plantation);
            }

            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
            return View(plantation);
        }

        // GET: Plantation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantation plantation = db.Plantations.Find(id);
            if (plantation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
            return View(plantation);
        }

        // POST: Plantation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlantationID,FieldID,CropTypeID,CropCycleID,FieldStageID,DatePlanted,RefugeSeedAmntUsed,RefugeUnit,RefugeAreaHectares,ExpectedYieldQnty,YieldUnit,DateHarvested,PlantationStatus")] Plantation plantation)
        {
            if (ModelState.IsValid)
            {
                var myPlantation = db.Plantations.Where(p => p.PlantationID == plantation.PlantationID).SingleOrDefault();

                if (myPlantation != null)
                {
                    myPlantation.PlantationID = plantation.PlantationID;
                    myPlantation.FieldID = plantation.FieldID;
                    myPlantation.CropTypeID = plantation.CropTypeID;
                    myPlantation.CropCycleID = plantation.CropCycleID;
                    myPlantation.FieldStageID = plantation.FieldStageID;
                    myPlantation.DatePlanted = plantation.DatePlanted;
                    myPlantation.RefugeSeedAmntUsed = plantation.RefugeSeedAmntUsed;
                    myPlantation.RefugeUnit = plantation.RefugeUnit;
                    myPlantation.RefugeAreaHectares = plantation.RefugeAreaHectares;
                    myPlantation.ExpectedYieldQnty = plantation.ExpectedYieldQnty;
                    myPlantation.YieldUnit = plantation.YieldUnit;
                    myPlantation.DateHarvested = plantation.DateHarvested;
                    myPlantation.PlantationStatus = "Planned";

                    db.SaveChanges();
                    plantation.JavaScriptToRun = "mySuccess()";
                    TempData["yay"] = plantation;

                    ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
                    ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
                    ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
                    ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
                    ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
                    ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
                    return View(plantation);
                }
            }

            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
            return View(plantation);
        }

        // GET: Plantation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantation plantation = db.Plantations.Find(id);
            if (plantation == null)
            {
                return HttpNotFound();
            }
            return View(plantation);
        }

        // POST: Plantation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plantation plantation = db.Plantations.Find(id);
            db.Plantations.Remove(plantation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion


        public ActionResult Set(int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Plantation plantation = db.Plantations.Find(id);

                plantation.FieldStageID = 5;
                plantation.JavaScriptToRun = "mySuccess()";
                if (plantation == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Plantations", plantation);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

            }
            return View();

        }

        #region Confirm Plantation
        //GET
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantation plantation = db.Plantations.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
            return View(plantation);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm([Bind(Include = "PlantationID,FieldID,CropTypeID,CropCycleID,FieldStageID,DatePlanted,RefugeSeedAmntUsed,RefugeUnit,RefugeAreaHectares,ExpectedYieldQnty,YieldUnit,DateHarvested,PlantationStatus")] Plantation plantation)
        {
            if (ModelState.IsValid)
            {
                var myPlantation = db.Plantations.Where(p => p.PlantationID == plantation.PlantationID).SingleOrDefault();
                using (FarmDbContext dc = new FarmDbContext())
                {
                    //Add plantation
                    //plantation.FieldStageID = 1; //Stage 1 - Planted
                    //plantation.PlantationStatus = "Confirmed";
                    //dc.Plantations.Add(plantation);
                    //dc.SaveChanges();
                    //Update the field status

                    //return RedirectToAction("Index");
                    myPlantation.PlantationID = plantation.PlantationID;
                    myPlantation.FieldID = plantation.FieldID;
                    myPlantation.CropTypeID = plantation.CropTypeID;
                    myPlantation.CropCycleID = plantation.CropCycleID;
                    myPlantation.FieldStageID = 1; //Stage 1 -Planned
                    myPlantation.DatePlanted = plantation.DatePlanted;
                    myPlantation.RefugeSeedAmntUsed = plantation.RefugeSeedAmntUsed;
                    myPlantation.RefugeUnit = plantation.RefugeUnit;
                    myPlantation.RefugeAreaHectares = plantation.RefugeAreaHectares;
                    myPlantation.ExpectedYieldQnty = plantation.ExpectedYieldQnty;
                    myPlantation.YieldUnit = plantation.YieldUnit;
                    myPlantation.DateHarvested = plantation.DateHarvested;
                    myPlantation.PlantationStatus = "Confirmed";

                    db.SaveChanges();
                    plantation.JavaScriptToRun = "mySuccess()";
                    TempData["yay"] = plantation;

                    TempData["plantationId"] = plantation.PlantationID;

                    ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
                    ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
                    ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
                    ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
                    ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
                    ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
                    //return View(plantation);
                    return RedirectToAction("InitTreat", "Plantation");
                }

            }

            int Id = plantation.FieldID;
            var field = db.Fields.Find(Id);
            field.FieldStatusID = 2; //In-Use
            db.Entry(field).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.CropCycleID = new SelectList(db.CropCycles, "CropCycleID", "CropCycleDescr", plantation.CropCycleID);
            ViewBag.CropTypeID = new SelectList(db.CropTypes, "CropTypeID", "CropTypeDescr", plantation.CropTypeID);
            ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FieldName", plantation.FieldID);
            ViewBag.FieldStageID = new SelectList(db.FieldStages, "FieldStageID", "FieldStageDescr", plantation.FieldStageID);
            ViewBag.RefugeUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.RefugeUnit);
            ViewBag.YieldUnit = new SelectList(db.Units, "UnitID", "UnitDescr", plantation.YieldUnit);
            return RedirectToAction("InitTreat","TreatPlantation");
            //return View(plantation);
        }

        #endregion


        #region Add Harvest to Silo
        //GET
        public ActionResult Harvest(int? id) //plantationID
        {
            if (id != null)
            {
                TempData["plantationId"] = id;
            }
            using (FarmDbContext dc = new FarmDbContext())
            {
                //var Id = dc.Plantations.Find(id);
                routeId = Convert.ToInt32(id); //pass this Id at the end to get the field
            }
            ViewBag.PlantationID = new SelectList(db.Plantations, "PlantationID", "PlantationStatus");
            ViewBag.SiloID = new SelectList(db.Silos, "SiloID", "SiloDescr");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Harvest([Bind(Include = "SiloHarvestID,SiloHarvestStoreStartDate,SiloHarvestStoreEndDate,SiloHarvestTonnesStored,SiloID,PlantationID")] SiloHarvest siloHarvest)
        {

            if (ModelState.IsValid)
            {
                int plantationID = (int)TempData["plantationId"];
                siloHarvest.PlantationID = plantationID;
                db.SiloHarvests.Add(siloHarvest);
                db.SaveChanges();

                //Calculate difference between ExpectedYield and SiloHarvestTonnes stored
                using (FarmDbContext dc = new FarmDbContext())
                {
                    var plantation = dc.Plantations.Find(plantationID);
                    var expYield = plantation.ExpectedYieldQnty;
                    var siloHarvestTStored = siloHarvest.SiloHarvestTonnesStored;
                    var yieldDiff = expYield - siloHarvestTStored;
                    //float yieldDiff = plantation.ExpectedYieldQnty - siloHarvest.SiloHarvestTonnesStored;
                    //if (yieldDiff > 0)
                    //{
                    //    return RedirectToAction("Reason");
                    //    //return Reason(plantationID);
                    //}
                }

                return RedirectToAction("Index", "Plantation");
            }

            ViewBag.PlantationID = new SelectList(db.Plantations, "PlantationID", "PlantationStatus", siloHarvest.PlantationID);
            ViewBag.SiloID = new SelectList(db.Silos, "SiloID", "SiloDescr", siloHarvest.SiloID);
            return View(siloHarvest);
        }

        #endregion

        #region Initial treatment

        // GET: TreatPlantation/Treat/1
        public ActionResult InitTreat(int? id) //plantationID
        {
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr");
            ViewBag.TreatmentID = new SelectList(db.Treatments.Where(t => !t.TreatmentDescr.Contains("Stage 2 (Secondary) Treatment") && !t.TreatmentDescr.Contains("Ad-Hoc Treatment")), "TreatmentID", "TreatmentDescr");
            ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("KM") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("ML")), "UnitID", "UnitDescr");
            int plantationID = (int)TempData["plantationId"];
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //GET PlantationID & FieldID
            using (FarmDbContext dc = new FarmDbContext())
            {
                var tplantation = dc.Plantations.Find(plantationID); //  plantation id
                //InitTreat(null, plantationID);
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
        public ActionResult InitTreat([Bind(Include = "TreatmentID,InventoryID,TreatmentQnty,Unit")] InventoryTreatment inventoryTreatment, int? id)
        {
            if (ModelState.IsValid)
            {
                //id = (int)TempData["plantationId"];
                //get inventory quantity: Treatment quantity for selected inventory item cannot be more than the available qty
                var qty = db.Inventories.Find(inventoryTreatment.InventoryID);
                try
                {
                    if (Convert.ToInt32(inventoryTreatment.TreatmentQnty) > Convert.ToInt32(qty.InvQty))
                    {
                        //return error message: Selected quantity cannot be more than the available inventory qty

                        ModelState.AddModelError("TreatmentQnty", "Selected quantity cannot be more than the available inventory quantity");

                        ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
                        ViewBag.TreatmentID = new SelectList(db.Treatments.Where(t => !t.TreatmentDescr.Contains("Stage 1 (Initial) Treatment")), "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID); //Treatment can only be stage 2 or Ad-Hoc
                        ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("KM") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("ML")), "UnitID", "UnitDescr", inventoryTreatment.Unit);

                        return View(inventoryTreatment);
                    }
                    else
                    {
                        db.InventoryTreatments.Add(inventoryTreatment);
                        db.SaveChanges();
                        inventoryTreatment.JavaScriptToRun = "mySuccess()";
                    }
                }
                catch (Exception e)
                {

                    ViewBag.Error = e.Message;
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


                            f.FieldStatusID = 2; //Treated (This should be field Stage SIYA!!!)
                            //db.Entry(inventoryTreatment).State = EntityState.Modified;
                            dc.Entry(f).State = EntityState.Modified;
                            dc.SaveChanges();
                        }
                        catch (Exception err)
                        {

                            ViewBag.Error = err.Message;
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
                        ViewBag.Error = e.Message;
                    }

                }


                //CreateFieldTreatment(inventoryTreatment.TreatmentID)
                return RedirectToAction("Plantations", "Plantation");
            }

            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventoryTreatment.Unit);
            return View(inventoryTreatment);
        }

        #endregion


        #region Treat Plantation
        public FieldTreatment ft = new FieldTreatment();

        // GET: TreatPlantation/Treat/1
        public ActionResult Treat(int? id) //plantationID
        {
            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr");
            ViewBag.TreatmentID = new SelectList(db.Treatments.Where(t => !t.TreatmentDescr.Contains("Stage 1 (Initial) Treatment")), "TreatmentID", "TreatmentDescr");
            ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("KM") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("ML")), "UnitID", "UnitDescr");
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
                //get inventory quantity: Treatment quantity for selected inventory item cannot be more than the available qty
                var qty = db.Inventories.Find(inventoryTreatment.InventoryID);
                try
                {
                    if(Convert.ToInt32(inventoryTreatment.TreatmentQnty) > Convert.ToInt32(qty.InvQty))
                    {
                        //return error message: Selected quantity cannot be more than the available inventory qty

                        ModelState.AddModelError("TreatmentQnty", "Selected quantity cannot be more than the available inventory quantity");

                        ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
                        ViewBag.TreatmentID = new SelectList(db.Treatments.Where(t => !t.TreatmentDescr.Contains("Stage 1 (Initial) Treatment")), "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID); //Treatment can only be stage 2 or Ad-Hoc
                        ViewBag.Unit = new SelectList(db.Units.Where(u=> !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("KM") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("ML")), "UnitID", "UnitDescr", inventoryTreatment.Unit);

                        return View(inventoryTreatment);
                    }
                    else
                    {
                        db.InventoryTreatments.Add(inventoryTreatment);
                        db.SaveChanges();
                        inventoryTreatment.JavaScriptToRun = "mySuccess()";
                    }
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


                            f.FieldStatusID = 2; //Treated (This should be field Stage SIYA!!!)
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
                return RedirectToAction("Plantations","Plantation");
            }

            ViewBag.InventoryID = new SelectList(db.Inventories, "InventoryID", "InvDescr", inventoryTreatment.InventoryID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentDescr", inventoryTreatment.TreatmentID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventoryTreatment.Unit);
            return View(inventoryTreatment);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        public JsonResult GetEvents()
        {
            using (FarmDbContext f = new FarmDbContext())
            {
                var events = f.Plantations.Where(a => a.PlantationStatus == "Planned").ToList();
                var newData = (from ta in events
                               join ti in f.CropTypes.ToList() on ta.CropTypeID equals ti.CropTypeID
                               join fi in f.Fields.ToList() on ta.FieldID equals fi.FieldID
                               join cc in f.CropCycles.ToList() on ta.CropCycleID equals cc.CropCycleID
                               join fs in f.FieldStages.ToList() on ta.FieldStageID equals fs.FieldStageID
                               join u in f.Units.ToList() on ta.RefugeUnit equals u.UnitID
                               join yu in f.Units.ToList() on ta.YieldUnit equals yu.UnitID
                               select new
                               {
                                   PlantationID = ta.PlantationID,
                                   PlantationStatus = ta.PlantationStatus,
                                   DatePlanted = ta.DatePlanted,
                                   DateHarvested = ta.DateHarvested,
                                   RefugeSeedAmntUsed = ta.RefugeSeedAmntUsed,
                                   RefugeAreaHectares = ta.RefugeAreaHectares,
                                   ExpectedYieldQnty = ta.ExpectedYieldQnty,
                                   CropName = ti.CropTypeDescr,
                                   FieldName = fi.FieldName,
                                   CycleName = cc.CropCycleDescr,
                                   Stage = fs.FieldStageDescr,
                                   Refuge = u.UnitDescr,
                                   YieldUnit = yu.UnitDescr
                               }).ToList();
                var myResult = new JsonResult();
                myResult = new JsonResult { Data = newData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                f.Configuration.LazyLoadingEnabled = false;
                return Json(newData, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetEventsConfirmed()
        {
            using (FarmDbContext f = new FarmDbContext())
            {
                var events = f.Plantations.Where(a => a.PlantationStatus == "Confirmed").ToList();
                var newData = (from ta in events
                               join ti in f.CropTypes.ToList() on ta.CropTypeID equals ti.CropTypeID
                               join fi in f.Fields.ToList() on ta.FieldID equals fi.FieldID
                               join cc in f.CropCycles.ToList() on ta.CropCycleID equals cc.CropCycleID
                               join fs in f.FieldStages.ToList() on ta.FieldStageID equals fs.FieldStageID
                               join u in f.Units.ToList() on ta.RefugeUnit equals u.UnitID
                               join yu in f.Units.ToList() on ta.YieldUnit equals yu.UnitID
                               select new
                               {
                                   PlantationID = ta.PlantationID,
                                   PlantationStatus = ta.PlantationStatus,
                                   DatePlanted = ta.DatePlanted,
                                   DateHarvested = ta.DateHarvested,
                                   RefugeSeedAmntUsed = ta.RefugeSeedAmntUsed,
                                   RefugeAreaHectares = ta.RefugeAreaHectares,
                                   ExpectedYieldQnty = ta.ExpectedYieldQnty,
                                   CropName = ti.CropTypeDescr,
                                   FieldName = fi.FieldName,
                                   CycleName = cc.CropCycleDescr,
                                   Stage = fs.FieldStageDescr,
                                   Refuge = u.UnitDescr,
                                   YieldUnit = yu.UnitDescr
                               }).ToList();
                var myResult = new JsonResult();
                myResult = new JsonResult { Data = newData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                f.Configuration.LazyLoadingEnabled = false;
                return Json(newData, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveEvent(Plantation e)
        {
            var status = false;
            using (FarmDbContext f = new FarmDbContext())
            {
                if (e.PlantationID > 0)
                {
                    //Update the event
                    var v = f.Plantations.Where(a => a.PlantationID == e.PlantationID).FirstOrDefault();
                    if (v != null)
                    {
                        v.PlantationID = e.PlantationID;
                        v.FieldID = e.FieldID;
                        v.CropTypeID = e.CropTypeID;
                        v.CropCycleID = e.CropCycleID;
                        v.FieldStageID = e.FieldStageID;
                        v.DatePlanted = e.DatePlanted;
                        v.RefugeSeedAmntUsed = e.RefugeSeedAmntUsed;
                        v.RefugeUnit = e.RefugeUnit;
                        v.RefugeAreaHectares = e.RefugeAreaHectares;
                        v.ExpectedYieldQnty = e.ExpectedYieldQnty;
                        v.YieldUnit = e.YieldUnit;
                        v.DateHarvested = e.DateHarvested;
                        v.PlantationStatus = e.PlantationStatus;
                    }
                }
                else
                {
                    f.Plantations.Add(e);
                }
                f.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }


        #region Redirect Functions
        [HttpPost]
        public ActionResult RedirectToEdit(int plantationID)
        {
            string myUrl = "/Plantation/Edit/" + plantationID;
            return Json(new { Url = myUrl });
        }

        [HttpPost]
        public ActionResult RedirectToSet(int plantationID)
        {
            string myUrl = "/Plantation/Set/" + plantationID;
            return Json(new { Url = myUrl });
        }

        [HttpPost]
        public ActionResult RedirectToConfirm(int plantationID)
        {
            string myUrl = "/Plantation/Confirm/" + plantationID;
            return Json(new { Url = myUrl });
        }

        [HttpPost]
        public ActionResult RedirectToHarvest(int? plantationID)
        {
            string url = "/Plantation/Harvest/" + plantationID;
            return Json(new { Url = url });
        }
        public ActionResult RedirectToTreat(int? plantationID)
        {
            string url = "/Plantation/Treat/" + plantationID;
            return Json(new { Url = url });
        }

        #endregion


    }
}