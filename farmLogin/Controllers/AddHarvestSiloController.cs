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
    public class AddHarvestSiloController : Controller
    {
        public static int routeId;

        private FarmDbContext db = new FarmDbContext();

        // GET: AddHarvestSilo
        //public ActionResult Index()
        //{
        //    var siloHarvests = db.SiloHarvests.Include(s => s.Plantation).Include(s => s.Silo);
        //    return View(siloHarvests.ToList());
        //}

        //// GET: AddHarvestSilo/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SiloHarvest siloHarvest = db.SiloHarvests.Find(id);
        //    if (siloHarvest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(siloHarvest);
        //}

        // GET: AddHarvestSilo/Create
        public ActionResult Create(int? id) //plantationID
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



        // POST: AddHarvestSilo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SiloHarvestID,SiloHarvestStoreStartDate,SiloHarvestStoreEndDate,SiloHarvestTonnesStored,SiloID,PlantationID")] SiloHarvest siloHarvest)
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
                    if (yieldDiff > 0)
                    {
                        //return RedirectToAction("Reason");
                        //return Reason(plantationID);
                        Reason(plantationID);
                    }
                }

                return RedirectToAction("Index", "Plantation");
            }

            ViewBag.PlantationID = new SelectList(db.Plantations, "PlantationID", "PlantationStatus", siloHarvest.PlantationID);
            ViewBag.SiloID = new SelectList(db.Silos, "SiloID", "SiloDescr", siloHarvest.SiloID);
            return View(siloHarvest);
        }

        public ActionResult Reason(int? id) //plantationId
        {
            //TempData["plantation"] = id;
            id = (int)TempData["plantationId"];
            return View();
        }

        public ActionResult FieldBioDisaster()
        {
            ViewBag.NatDisasterID = new SelectList(db.NaturalDisasters, "NatDisasterID", "NatDisasterDescr");
            ViewBag.BioDisasterID = new SelectList(db.BiologicalDisasters, "BioDisasterID", "BioDisasterDescr");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FieldBioDisaster(BiologicalDisaster biodisaster)
        {
            FieldBiologicalDisaster model = new FieldBiologicalDisaster();
            //biodisaster.BioDisasterDescr = db.BiologicalDisasters.Where(p => p.BioDisasterID == biodisaster.BioDisasterID).ToString();
            //if (ModelState.IsValid)
            //{
            using (FarmDbContext dc = new FarmDbContext())
            {

                model.Date = System.DateTime.Now;
                int f = (int)TempData["plantationId"];
                var fld = dc.Plantations.Find(f);
                int fId = fld.FieldID;
                model.FieldID = f;
                model.BioDisasterID = biodisaster.BioDisasterID;

                //populate FieldNatural Disaster table
                dc.FieldBiologicalDisasters.Add(model);
                dc.SaveChanges();
                //routeId = 0; //reset
                return RedirectToAction("SpecifyHarvestComplete");
            }

            //}
            //else
            //{
            //    ViewBag.Message = "Someting went wrong!";
            //}

            //ViewBag.NatDisasterID = new SelectList(db.NaturalDisasters, "NatDisasterID", "NatDisasterDescr");
            //ViewBag.BioDisasterID = new SelectList(db.BiologicalDisasters, "BioDisasterID", "BioDisasterDescr");
            //return View();
        }
        //GET
        public ActionResult FieldNaturalDisaster()   //return the view FieldNaturalDisaster
        {
            //get field Id
            //int fplantation = (int)TempData["plantation"];
            //using (FarmDbContext dc = new FarmDbContext())
            //{
            //    var f = dc.Plantations.Find(fplantation);
            //    int fId = f.FieldID; //got my field
            //    ViewBag.FieldID = fId; //store in viewBag
            //}
            //ViewBag.FieldID = new SelectList(db.Fields, "FieldID", "FielDescr");
            ViewBag.NatDisasterID = new SelectList(db.NaturalDisasters, "NatDisasterID", "NatDisasterDescr");
            ViewBag.BioDisasterID = new SelectList(db.BiologicalDisasters, "BioDisasterID", "BioDisasterDescr");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FieldNaturalDisaster(NaturalDisaster naturaldisaster)
        {
            FieldNaturalDisaster model = new FieldNaturalDisaster(); //populate this obj and save in db
            //assgin variables and save to db
            using (FarmDbContext dc = new FarmDbContext())
            {
                model.Date = System.DateTime.Now;
                int f = routeId;
                var fld = dc.Plantations.Find(f);
                int fId = fld.FieldID;
                model.FieldID = f;
                model.NatDisasterID = naturaldisaster.NatDisasterID;



                //populate FieldNatural Disaster table
                dc.FieldNaturalDisasters.Add(model);
                dc.SaveChanges();
                //routeId = 0; //reset
                return RedirectToAction("SpecifyHarvestComplete");
            }
            //ViewBag.NatDisasterID = new SelectList(db.NaturalDisasters, "NatDisasterID", "NatDisasterDescr");
            //ViewBag.BioDisasterID = new SelectList(db.BiologicalDisasters, "BioDisasterID", "BioDisasterDescr");
            //return View();
        }

        public ActionResult SpecifyHarvestComplete()
        {
            return View();
        }
        public ActionResult PartialHarvest()
        {
            using (FarmDbContext dc = new FarmDbContext())
            {
                var plantation = dc.Plantations.Find(routeId);
                //if (ModelState.IsValid)
                //{
                if (plantation != null)
                {
                    //var plantation = dc.Plantations.Find(routeId);
                    try
                    {
                        // plantation.PlantationStatus = "Complete";
                        dc.Entry(plantation).State = EntityState.Modified;
                        dc.SaveChanges();
                    }
                    catch (Exception err)
                    {

                        ViewBag.Message = err.Message;
                    }
                    return RedirectToAction("Index", "Plantation"); //success
                }
                else
                {
                    ViewBag.Message = "Model is not valid";
                }

                //routeId = 0; //empty
                RedirectToAction("Index", "Plantation");
            }
            return View();
        }

        public ActionResult NotApplicable()
        {
            return RedirectToAction("Create");
        }

        public ActionResult CompleteHarvest()
        {
            using (FarmDbContext dc = new FarmDbContext())
            {
                int plantationID = (int)TempData["plantationId"];
                var plantation = dc.Plantations.Find(plantationID);
                //if (ModelState.IsValid)
                //{
                if (plantation != null)
                {
                    //var plantation = dc.Plantations.Find(routeId);
                    try
                    {
                        //Update Plantation Status to "Complete"
                        plantation.PlantationStatus = "Complete";
                        dc.Entry(plantation).State = EntityState.Modified;

                        //Update Field Status = "In preparation"
                        var field = dc.Fields.Find(plantation.FieldID);
                        field.FieldStatusID = 1; //"In preparation";
                        dc.Entry(field).State = EntityState.Modified;

                        dc.SaveChanges();
                        //RedirectToAction("Index", "Plantation");
                    }
                    catch (Exception err)
                    {

                        ViewBag.Message = err.Message;
                    }

                }
                else
                {
                    ViewBag.Message = "Model is not valid";
                }

                //routeId = 0; //empty
                return RedirectToAction("Index", "Plantation", null);
            }
            //return View();
        }






        //// GET: AddHarvestSilo/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SiloHarvest siloHarvest = db.SiloHarvests.Find(id);
        //    if (siloHarvest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.PlantationID = new SelectList(db.Plantations, "PlantationID", "PlantationStatus", siloHarvest.PlantationID);
        //    ViewBag.SiloID = new SelectList(db.Silos, "SiloID", "SiloDescr", siloHarvest.SiloID);
        //    return View(siloHarvest);
        //}

        //// POST: AddHarvestSilo/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SiloHarvestID,SiloHarvestStoreStartDate,SiloHarvestStoreEndDate,SiloHarvestTonnesStored,SiloID,PlantationID")] SiloHarvest siloHarvest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(siloHarvest).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.PlantationID = new SelectList(db.Plantations, "PlantationID", "PlantationStatus", siloHarvest.PlantationID);
        //    ViewBag.SiloID = new SelectList(db.Silos, "SiloID", "SiloDescr", siloHarvest.SiloID);
        //    return View(siloHarvest);
        //}

        //// GET: AddHarvestSilo/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SiloHarvest siloHarvest = db.SiloHarvests.Find(id);
        //    if (siloHarvest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(siloHarvest);
        //}

        //// POST: AddHarvestSilo/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SiloHarvest siloHarvest = db.SiloHarvests.Find(id);
        //    db.SiloHarvests.Remove(siloHarvest);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
