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
    public class VehicleServiceController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: VehicleService
        public ActionResult Index()
        {
            VehicleService vs = new VehicleService();
            
            return View(vs);
        }

        // GET: VehicleService/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleService vehicleService = db.VehicleServices.Find(id);
            if (vehicleService == null)
            {
                return HttpNotFound();
            }
            return View(vehicleService);
        }

        // GET: VehicleService/Create
        public ActionResult Create()
        {
            VehicleService vehservice = new VehicleService();
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehservice.UnitID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehservice.VehicleID);

            return View(vehservice);
        }

        // POST: VehicleService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "VehicleServiceID,VehicleService_Date,VehicleService_Cost,VehicleServiceRecord,UnitID,VehicleID")]*/ VehicleService vehicleService)
        {
            if (ModelState.IsValid)
            {
                db.VehicleServices.Add(vehicleService);
                db.SaveChanges();
                //ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
                ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
                ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehicleService.VehicleID);
                vehicleService.JavaScriptToRun = "mySuccess()";
                return View(vehicleService);
            }

            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehicleService.VehicleID);
            return View(vehicleService);
        }

        // GET: VehicleService/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleService vehicleService = db.VehicleServices.Find(id);
            if (vehicleService == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehicleService.VehicleID);
            return View(vehicleService);
        }

        // POST: VehicleService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "VehicleServiceID,VehicleService_Date,VehicleService_Cost,VehicleServiceRecord,UnitID,VehicleID")]*/ VehicleService vehicleService)
        {
            //vehicleService = db.VehicleServices.Find(vehicleService.VehicleServiceID);
            


            if (ModelState.IsValid)
            {

                //var vs = db.VehicleServices.Where(v => v.VehicleServiceID == vehicleService.VehicleServiceID).SingleOrDefault();

                
                    //VehicleService entity = new VehicleService();
                    //entity.VehicleServiceID = vs.VehicleServiceID;
                    //entity.VehicleService_Date = vs.VehicleService_Date;
                    //entity.VehicleService_Cost = vs.VehicleService_Cost;
                    //entity.VehicleServiceRecord = vs.VehicleServiceRecord;
                    //entity.UnitID = vs.UnitID;
                    //entity.VehicleID = vs.VehicleID;

                    db.Entry(vehicleService).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
                    ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehicleService.VehicleID);
                    vehicleService.JavaScriptToRun = "mySuccess()";
                    return View(vehicleService);
                


            }
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicleService.UnitID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehName", vehicleService.VehicleID);
            return View(vehicleService);
        }

        // GET: VehicleService/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleService vehicleService = db.VehicleServices.Find(id);
            if (vehicleService == null)
            {
                return HttpNotFound();
            }
            return View(vehicleService);
        }

        // POST: VehicleService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleService vehicleService = db.VehicleServices.Find(id);
            db.VehicleServices.Remove(vehicleService);
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
    }
}
