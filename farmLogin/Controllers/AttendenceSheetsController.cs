using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using Microsoft.AspNet.Identity;

namespace farmLogin.Controllers
{
    public class AttendenceSheetsController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: AttendenceSheets
        public ActionResult Index()
        {
            var attendenceSheets = db.AttendenceSheets.Include(a => a.FarmWorker).Include(a => a.User);
            return View(attendenceSheets.ToList());
        }

        // GET: AttendenceSheets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendenceSheet attendenceSheet = db.AttendenceSheets.Find(id);
            if (attendenceSheet == null)
            {
                return HttpNotFound();
            }
            return View(attendenceSheet);
        }

        ///clock out
                // GET: AttendenceSheets/Create
        public ActionResult clockOut()
        {
            AttendenceSheet at = new AttendenceSheet();


            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress");
            return View(at);
        }

        // POST: AttendenceSheets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult clockOut(/*[Bind(Include = "AttendenceSheetID,ClockInTime,ClockOutTime,FarmWorkerNum,UserID")]*/ AttendenceSheet attendenceSheet)
        {
            if (ModelState.IsValid)
            {
                var attendanceList = db.AttendenceSheets.Where(a => a.FarmWorkerNum == attendenceSheet.FarmWorkerNum).OrderByDescending(a => a.AttendenceSheetID);
                var attendanceLast = attendanceList.FirstOrDefault();

                if (attendanceLast != null && attendanceLast.ClockInTime != null && attendanceLast.ClockOutTime == null)
                {
                    //attendanceLast.ClockOutTime = DateTime.Now;
                    attendanceLast.ClockOutTime = attendenceSheet.ClockOutTime;
                    db.Entry(attendanceLast).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
                    return View(attendenceSheet);
                }

                //var myItem = db.AttendenceSheets.Where(a => a.FarmWorkerNum == attendenceSheet.FarmWorkerNum && a.ClockInTime != null && a.ClockOutTime == null).First();
                //myItem.ClockOutTime = DateTime.Now;

                //db.Entry(myItem).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
            return View(attendenceSheet);
        }











        // GET: AttendenceSheets/Create
        public ActionResult Create()
        {
            AttendenceSheet at = new AttendenceSheet();

            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName");
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress");
            return View(at);
        }

        // POST: AttendenceSheets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "AttendenceSheetID,ClockInTime,ClockOutTime,FarmWorkerNum,UserID")]*/ AttendenceSheet attendenceSheet)
        {
            if (ModelState.IsValid)
            {
                //get list of all users
                var uList = db.Users.ToList();

                //get the email of currently logged in email
                var email = User.Identity.GetUserName();

                //user email to find the id of currently logged in user
                var currID = uList.Where(a => a.UserEmailAddress == email).Select(a => a.UserID).FirstOrDefault();
                
                //set clockin time and current user id then save to db
                attendenceSheet.ClockInTime = DateTime.Now;
                attendenceSheet.UserID = currID;
                db.AttendenceSheets.Add(attendenceSheet);
                db.SaveChanges();

                attendenceSheet.JavaScriptToRun = "mySuccess()";
                ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
                return View(attendenceSheet);
            }

            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
            return View(attendenceSheet);
        }

        // GET: AttendenceSheets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendenceSheet attendenceSheet = db.AttendenceSheets.Find(id);
            if (attendenceSheet == null)
            {
                return HttpNotFound();
            }
            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
            return View(attendenceSheet);
        }

        // POST: AttendenceSheets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendenceSheetID,ClockInTime,ClockOutTime,FarmWorkerNum,UserID")] AttendenceSheet attendenceSheet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendenceSheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FarmWorkerNum = new SelectList(db.FarmWorkers, "FarmWorkerNum", "FarmWorkerFName", attendenceSheet.FarmWorkerNum);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmailAddress", attendenceSheet.UserID);
            return View(attendenceSheet);
        }

        // GET: AttendenceSheets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendenceSheet attendenceSheet = db.AttendenceSheets.Find(id);
            if (attendenceSheet == null)
            {
                return HttpNotFound();
            }
            return View(attendenceSheet);
        }

        // POST: AttendenceSheets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttendenceSheet attendenceSheet = db.AttendenceSheets.Find(id);
            db.AttendenceSheets.Remove(attendenceSheet);
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
