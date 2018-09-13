using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Controllers
{
    public class RainfallRecordController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: RainfallRecord/Create
        public ActionResult CaptureRainfallRec()
        {
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName");
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr");
            var rain = new RainfallRecord();
            return View(rain);
        }

        // POST: RainfallRecord/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaptureRainfallRec([Bind(Include = "RainFallRecID,RecordDate,RecordMeasurement,UnitID,LandID")] RainfallRecord rainfallRecord)
        {
            if (ModelState.IsValid)
            {
                db.RainfallRecords.Add(rainfallRecord);
                db.SaveChanges();
                return RedirectToAction("Index", "Farm");
            }

            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", rainfallRecord.LandID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", rainfallRecord.UnitID);
            return View(rainfallRecord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class MyDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
            {
                DateTime d = Convert.ToDateTime(value);
                return d >= DateTime.Now; //Dates Greater than or equal to today are valid (true)

            }
        }
    }
}
