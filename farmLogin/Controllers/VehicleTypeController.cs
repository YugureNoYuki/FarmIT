using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class VehicleTypeController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: VehicleType
        public ActionResult Index()
        {
            VehicleType vehType = new VehicleType();


            if (TempData["yay"] != null)
            {
                vehType = (VehicleType)TempData["yay"];
                TempData.Remove("yay");
            }


            return View(vehType);
        }

        // GET: VehicleType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // GET: VehicleType/Create
        public ActionResult Create()
        {
            VehicleType vehType = new VehicleType();


            if(TempData["yay"] != null)
            {
                vehType = (VehicleType)TempData["yay"];
                TempData.Remove("yay");
            }
            return View(vehType);
        }

        // POST: VehicleType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleType vehicleType, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {


                //if (file == null)
                //{
                //    ViewBag.Error = "File is Empty!";
                //}
                //else
                //{
                //    //array of allowed extensions
                //    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                //    //checking extension of file uploaded
                //    var checkExtension = Path.GetExtension(file.FileName).ToLower();

                //    //check if does not contain the extension (not png/jpg/jpeg)
                //    if (!allowedExtensions.Contains(checkExtension))
                //    {
                //        ViewBag.Error = "Only PNG, JPG Files are allowed.";
                //        return View(vehicleType);

                //    }

                //    //var file = value as HttpPostedFileBase;
                //    if (file == null)
                //    {
                //        ViewBag.Error = "No file upload!";
                //    }

                //    if (file.ContentLength > 2 * 1024 * 1024)
                //    {
                //        ViewBag.Error = "File too big!";
                //    }

                //    try
                //    {
                //        using (var img = Image.FromStream(file.InputStream))
                //        {

                //            if (img.RawFormat.Equals(ImageFormat.Png) == false && img.RawFormat.Equals(ImageFormat.Jpeg) == false)
                //            {
                //                ViewBag.Error = "Not PNG/JPG Format!";
                //            }
                //        }
                //    }
                //    catch { }

                var nameExist = IsNameExist(vehicleType.VehTypeDescr);
                if (nameExist)
                {
                    ModelState.AddModelError("VehTypeExist", "Land already exist, please specify different Land Name.");
                    ViewBag.Error = "Vehicle Type already exists, specify a different Description.";
                    return View(vehicleType);
                }

                if (ViewBag.Error == null)
                    {

                        //string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                        //file.SaveAs(path);

                        //vehicleType.VehTypeImg = "~/Files/" + file.FileName;
                        db.VehicleTypes.Add(vehicleType);


                        db.SaveChanges();
                        vehicleType.JavaScriptToRun = "mySuccess()";
                        TempData["yay"] = vehicleType;

                        return View(vehicleType);
                        
                    }

                }

                vehicleType.JavaScriptToRun = "mySuccess()";
                return View(vehicleType);
            }

        [NonAction]
        public bool IsNameExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.VehicleTypes.Where(a => a.VehTypeDescr == inData).FirstOrDefault();
                return v != null;
            }
        }

        //  return View(vehicleType);


        // GET: VehicleType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }

            if (TempData["yay"] != null)
            {
                vehicleType = (VehicleType)TempData["yay"];
                TempData.Remove("yay");
            }
            return View(vehicleType);
        }

        // POST: VehicleType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehTypeID,VehTypeDescr,VehTypeImg")] VehicleType vehicleType, HttpPostedFileBase file)
        {
            //if (ModelState.IsValid)
            //{
            //    //db.Entry(vehicleType).State = EntityState.Modified;
            //    //db.SaveChanges();
            //    //return RedirectToAction("Index");
            //}

            if (ModelState.IsValid)
            {


                //if (file == null)
                //{
                //    ViewBag.Error = "File is Empty!";
                //}
                //else
                //{
                //    //array of allowed extensions
                //    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                //    //checking extension of file uploaded
                //    var checkExtension = Path.GetExtension(file.FileName).ToLower();

                //    //check if does not contain the extension (not png/jpg/jpeg)
                //    if (!allowedExtensions.Contains(checkExtension))
                //    {
                //        ViewBag.Error = "Only PNG, JPG Files are allowed.";
                //        return View(vehicleType);

                //    }

                //    //var file = value as HttpPostedFileBase;
                //    if (file == null)
                //    {
                //        ViewBag.Error = "No file upload!";
                //    }

                //    if (file.ContentLength > 2 * 1024 * 1024)
                //    {
                //        ViewBag.Error = "File too big!";
                //    }

                //    try
                //    {
                //        using (var img = Image.FromStream(file.InputStream))
                //        {

                //            if (img.RawFormat.Equals(ImageFormat.Png) == false && img.RawFormat.Equals(ImageFormat.Jpeg) == false)
                //            {
                //                ViewBag.Error = "Not PNG/JPG Format!";
                //            }
                //        }
                //    }
                //    catch { }

                    if (ViewBag.Error == null)
                    {

                        //string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                        //file.SaveAs(path);

                        //vehicleType.VehTypeImg = "~/Files/" + file.FileName;
                        db.VehicleTypes.Add(vehicleType);

                        db.Entry(vehicleType).State = EntityState.Modified;
                        db.SaveChanges();
                        vehicleType.JavaScriptToRun = "mySuccess()";
                        TempData["yay"] = vehicleType;

                        return View(vehicleType);

                        //db.SaveChanges();
                        //vehicleType.JavaScriptToRun = "mySuccess()";
                        //TempData["yay"] = vehicleType;

                        //return View(vehicleType);

                    }

                }

                return View(vehicleType);
            }
        

        // GET: VehicleType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            //return View(vehicleType);

            if (vehicleType.Vehicles.Count < 1 && vehicleType != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.VehicleTypes.Remove(vehicleType);
                    db.SaveChanges();

                    var model = db.VehicleTypes.Select(e => e);
                    //var myModel = model.ToList();
                    //myModel.

                    //InventoryIndexViewModel myModel = new InventoryIndexViewModel();

                    vehicleType.JavaScriptToRun = "myDelSuccess()";
                    //TempData["success"] = "success";
                    return View("Index", vehicleType);

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

                var model = db.InventoryTypes.Select(e => e);

                vehicleType.JavaScriptToRun = "myFail()";
                return View("Index", vehicleType);

                //return RedirectToAction("Index");
            }

            //RedirectToAction("Index");

            //return RedirectToAction("DeleteConfirmed", "InventoryType");
        }


        // POST: VehicleType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleType vehicleType = db.VehicleTypes.Find(id);
            db.VehicleTypes.Remove(vehicleType);
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
