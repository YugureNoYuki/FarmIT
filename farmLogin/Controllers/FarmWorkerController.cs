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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class FarmWorkerController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        public ActionResult HowToClockInOrOut()
        {
            return View();
        }

        // GET: FarmWorker
        public ActionResult Index(string farmworkertype, string search, int? page)
        {
            //instatiate new view model
            FarmWorkerIndexViewModel viewModel = new FarmWorkerIndexViewModel();

            //select farmworkers
            var farmWorkers = db.FarmWorkers.Include(f => f.Country).Include(f => f.Farm).Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Province).Include(f => f.Title); ;
            //var farmWorkers = db.FarmWorkers.Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Title);

            //do search and save search string in view model
            if (!String.IsNullOrEmpty(search))
            {
                farmWorkers = farmWorkers.Where(i => i.FarmWorkerFName.Contains(search) ||
                    i.FarmWorkerLName.Contains(search) || i.FarmWorkerType.FarmWorkerTypeDescr.Contains(search));
                viewModel.Search = search;
            }

            //group search results into types and count how many workers in each type
            viewModel.WorkerTypesWithCount = from matchingFarmWorkers in farmWorkers
                                                 //where
                                                 //matchingFarmWorkers.FarmWorkerTypeID != null
                                             group matchingFarmWorkers by
                                             matchingFarmWorkers.FarmWorkerType.FarmWorkerTypeDescr into
                                             FarmWorkerTypeGroup
                                             select new FarmWorkerTypesWithCount()
                                             {
                                                 FarmWorkerTypeDescr = FarmWorkerTypeGroup.Key,
                                                 FarmWorkerCount = FarmWorkerTypeGroup.Count()
                                             };

            if (!String.IsNullOrEmpty(farmworkertype))
            {
                farmWorkers = farmWorkers.Where(i => i.FarmWorkerType.FarmWorkerTypeDescr == farmworkertype);
                viewModel.FarmWorkerType = farmworkertype;
            }

            //sort results
            farmWorkers = farmWorkers.OrderBy(i => i.FarmWorkerLName);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.FarmWorkers = farmWorkers.ToPagedList(currentPage, PageItems);


            FarmWorker fw = new FarmWorker();

            if (TempData["yay"] != null)
            {
                fw = (FarmWorker)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(viewModel);
        }

        // GET: FarmWorker/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorker farmWorker = db.FarmWorkers.Find(id);
            if (farmWorker == null)
            {
                return HttpNotFound();
            }
            return View(farmWorker);
        }

        // GET: FarmWorker/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr");
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName");
            ViewBag.FarmWorkerTypeID = new SelectList(db.FarmWorkerTypes, "FarmWorkerTypeID", "FarmWorkerTypeDescr");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderDescr");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");
            ViewBag.TitleID = new SelectList(db.Titles, "TitleID", "TitleDescr");

            FarmWorker fw = new FarmWorker();

            if (TempData["yay"] != null)
            {
                fw = (FarmWorker)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(fw);
        }

        // POST: FarmWorker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FarmWorker farmWorker, HttpPostedFileBase file)
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", farmWorker.CountryID);
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", farmWorker.FarmID);
            ViewBag.FarmWorkerTypeID = new SelectList(db.FarmWorkerTypes, "FarmWorkerTypeID", "FarmWorkerTypeDescr", farmWorker.FarmWorkerTypeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderDescr", farmWorker.GenderID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farmWorker.ProvinceID);
            ViewBag.TitleID = new SelectList(db.Titles, "TitleID", "TitleDescr", farmWorker.TitleID);


            var isExist = IsIDExist(farmWorker.FarmWorkerIDNum);
            if (isExist)
            {
                ModelState.AddModelError("FarmWorkerExist", "Farmworker already exists, please specify unique ID Number.");
                ViewBag.Error = "Farm Worker already exists! Please specify unique ID number.";

                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", farmWorker.CountryID);
                ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", farmWorker.FarmID);
                ViewBag.FarmWorkerTypeID = new SelectList(db.FarmWorkerTypes, "FarmWorkerTypeID", "FarmWorkerTypeDescr", farmWorker.FarmWorkerTypeID);
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderDescr", farmWorker.GenderID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farmWorker.ProvinceID);
                ViewBag.TitleID = new SelectList(db.Titles, "TitleID", "TitleDescr", farmWorker.TitleID);
                return View(farmWorker);
            }

            if (ModelState.IsValid)
            {

                if (file == null)
                {
                    ViewBag.Error = "File is Empty!";
                    db.FarmWorkers.Add(farmWorker);


                    db.SaveChanges();
                    farmWorker.JavaScriptToRun = "mySuccess()";
                    return View(farmWorker);

                }
                else
                {
                    //array of allowed extensions
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                    //checking extension of file uploaded
                    var checkExtension = Path.GetExtension(file.FileName).ToLower();

                    //check if does not contain the extension (not png/jpg/jpeg)
                    if (!allowedExtensions.Contains(checkExtension))
                    {
                        ViewBag.Error = "Only PNG, JPG Files are allowed.";
                        return View(farmWorker);

                    }

                    //var file = value as HttpPostedFileBase;
                    if (file == null)
                    {
                        ViewBag.Error = "No file upload!";
                    }

                    if (file.ContentLength > 2 * 1024 * 1024)
                    {
                        ViewBag.Error = "File too big!";
                    }

                    try
                    {
                        using (var img = Image.FromStream(file.InputStream))
                        {

                            if (img.RawFormat.Equals(ImageFormat.Png) == false && img.RawFormat.Equals(ImageFormat.Jpeg) == false)
                            {
                                ViewBag.Error = "Not PNG/JPG Format!";
                            }
                        }
                    }
                    catch { }

                    if (ViewBag.Error == null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        farmWorker.FarmWorkerImg = "~/Files/" + file.FileName;
                        db.FarmWorkers.Add(farmWorker);


                        db.SaveChanges();
                        farmWorker.JavaScriptToRun = "mySuccess()";
                        TempData["yay"] = farmWorker;

                        return View(farmWorker);

                    }


                }

                //farmWorker.JavaScriptToRun = "mySuccess()";
                //return View(farmWorker);

            }
            return View(farmWorker);
        }


        // GET: FarmWorker/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorker farmWorker = db.FarmWorkers.Find(id);
            if (farmWorker == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", farmWorker.CountryID);
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", farmWorker.FarmID);
            ViewBag.FarmWorkerTypeID = new SelectList(db.FarmWorkerTypes, "FarmWorkerTypeID", "FarmWorkerTypeDescr", farmWorker.FarmWorkerTypeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderDescr", farmWorker.GenderID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farmWorker.ProvinceID);
            ViewBag.TitleID = new SelectList(db.Titles, "TitleID", "TitleDescr", farmWorker.TitleID);

            //FarmWorker fw = new FarmWorker();

            if (TempData["yay"] != null)
            {
                farmWorker = (FarmWorker)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(farmWorker);
        }

        // POST: FarmWorker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FarmWorkerNum,FarmWorkerFName,FarmWorkerLName,FarmWorkerContactNum,FarmWorkerImg,Address,Surburb,City,CountryID,ProvinceID,ContractStartDate,ContractEndDate,FarmWorkerIDNum,TitleID,GenderID,FarmWorkerTypeID,FarmID")] FarmWorker farmWorker, HttpPostedFileBase file)
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", farmWorker.CountryID);
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", farmWorker.FarmID);
            ViewBag.FarmWorkerTypeID = new SelectList(db.FarmWorkerTypes, "FarmWorkerTypeID", "FarmWorkerTypeDescr", farmWorker.FarmWorkerTypeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderDescr", farmWorker.GenderID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", farmWorker.ProvinceID);
            ViewBag.TitleID = new SelectList(db.Titles, "TitleID", "TitleDescr", farmWorker.TitleID);

            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ViewBag.Error = "File is Empty!";

                    var mw = db.FarmWorkers.Where(f => f.FarmWorkerNum == farmWorker.FarmWorkerNum).SingleOrDefault();

                    if (mw != null)
                    {
                        //string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                        //file.SaveAs(path);

                        //farmWorker.FarmWorkerImg = "~/Files/" + file.FileName;
                        //db.FarmWorkers.Add(farmWorker);

                        //db.Entry(farmWorker).State = EntityState.Modified;

                        mw.FarmWorkerNum = farmWorker.FarmWorkerNum;
                        mw.FarmWorkerFName = farmWorker.FarmWorkerFName;
                        mw.FarmWorkerLName = farmWorker.FarmWorkerLName;
                        mw.FarmWorkerContactNum = farmWorker.FarmWorkerContactNum;
                        mw.FarmWorkerImg = mw.FarmWorkerImg;
                        mw.Address = farmWorker.Address;
                        mw.Surburb = farmWorker.Surburb;
                        mw.City = farmWorker.City;
                        mw.CountryID = farmWorker.CountryID;
                        mw.ProvinceID = farmWorker.ProvinceID;
                        mw.ContractStartDate = farmWorker.ContractStartDate;
                        mw.ContractEndDate = farmWorker.ContractEndDate;
                        mw.FarmWorkerIDNum = farmWorker.FarmWorkerIDNum;
                        mw.TitleID = farmWorker.TitleID;
                        mw.GenderID = farmWorker.GenderID;
                        mw.FarmWorkerTypeID = farmWorker.FarmWorkerTypeID;
                        mw.FarmID = farmWorker.FarmID;


                        db.SaveChanges();
                        farmWorker.JavaScriptToRun = "mySuccess()";
                        TempData["yay"] = farmWorker;

                        return View(farmWorker);
                    }
                }
                else
                {
                    //array of allowed extensions
                    var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                    //checking extension of file uploaded
                    var checkExtension = Path.GetExtension(file.FileName).ToLower();

                    //check if does not contain the extension (not png/jpg/jpeg)
                    if (!allowedExtensions.Contains(checkExtension))
                    {
                        ViewBag.Error = "Only PNG, JPG Files are allowed.";
                        return View(farmWorker);

                    }

                    //var file = value as HttpPostedFileBase;
                    if (file == null)
                    {
                        ViewBag.Error = "No file upload!";
                    }

                    if (file.ContentLength > 2 * 1024 * 1024)
                    {
                        ViewBag.Error = "File too big!";
                    }

                    try
                    {
                        using (var img = Image.FromStream(file.InputStream))
                        {

                            if (img.RawFormat.Equals(ImageFormat.Png) == false && img.RawFormat.Equals(ImageFormat.Jpeg) == false)
                            {
                                ViewBag.Error = "Not PNG/JPG Format!";
                            }
                        }
                    }
                    catch { }

                    if (ViewBag.Error == null)
                    {

                        var myWorker = db.FarmWorkers.Where(f => f.FarmWorkerNum == farmWorker.FarmWorkerNum).SingleOrDefault();

                        if (myWorker != null)
                        {
                            string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
                            file.SaveAs(path);

                            //farmWorker.FarmWorkerImg = "~/Files/" + file.FileName;
                            //db.FarmWorkers.Add(farmWorker);

                            //db.Entry(farmWorker).State = EntityState.Modified;

                            myWorker.FarmWorkerNum = farmWorker.FarmWorkerNum;
                            myWorker.FarmWorkerFName = farmWorker.FarmWorkerFName;
                            myWorker.FarmWorkerLName = farmWorker.FarmWorkerLName;
                            myWorker.FarmWorkerContactNum = farmWorker.FarmWorkerContactNum;
                            myWorker.FarmWorkerImg = "~/Files/" + file.FileName;
                            myWorker.Address = farmWorker.Address;
                            myWorker.Surburb = farmWorker.Surburb;
                            myWorker.City = farmWorker.City;
                            myWorker.CountryID = farmWorker.CountryID;
                            myWorker.ProvinceID = farmWorker.ProvinceID;
                            myWorker.ContractStartDate = farmWorker.ContractStartDate;
                            myWorker.ContractEndDate = farmWorker.ContractEndDate;
                            myWorker.FarmWorkerIDNum = farmWorker.FarmWorkerIDNum;
                            myWorker.TitleID = farmWorker.TitleID;
                            myWorker.GenderID = farmWorker.GenderID;
                            myWorker.FarmWorkerTypeID = farmWorker.FarmWorkerTypeID;
                            myWorker.FarmID = farmWorker.FarmID;


                            db.SaveChanges();
                            farmWorker.JavaScriptToRun = "mySuccess()";
                            TempData["yay"] = farmWorker;

                            return View(farmWorker);
                        }

                    }

                    //farmWorker.JavaScriptToRun = "mySuccess()";
                    //return View(farmWorker);

                }

            }
            return View(farmWorker);
        }

        // GET: FarmWorker/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FarmWorker farmWorker = db.FarmWorkers.Find(id);
            if (farmWorker == null)
            {
                return HttpNotFound();
            }

            if (farmWorker != null && farmWorker.AttendenceSheets.Count < 1)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.FarmWorkers.Remove(farmWorker);
                    db.SaveChanges();

                    //var myModel = model.ToList();
                    //myModel.

                    //InventoryIndexViewModel myModel = new InventoryIndexViewModel();

                    //instatiate new view model
                    FarmWorkerIndexViewModel viewModel = new FarmWorkerIndexViewModel();

                    //select farmworkers
                    var farmWorkers = db.FarmWorkers.Include(f => f.Country).Include(f => f.Farm).Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Province).Include(f => f.Title); ;
                    //var farmWorkers = db.FarmWorkers.Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Title);

                    //do search and save search string in view model
                    if (!String.IsNullOrEmpty(""))
                    {
                        farmWorkers = farmWorkers.Where(i => i.FarmWorkerFName.Contains("") ||
                            i.FarmWorkerLName.Contains("") || i.FarmWorkerType.FarmWorkerTypeDescr.Contains(""));
                        viewModel.Search = "";
                    }

                    //group search results into types and count how many workers in each type
                    viewModel.WorkerTypesWithCount = from matchingFarmWorkers in farmWorkers
                                                         //where
                                                         //matchingFarmWorkers.FarmWorkerTypeID != null
                                                     group matchingFarmWorkers by
                                                     matchingFarmWorkers.FarmWorkerType.FarmWorkerTypeDescr into
                                                     FarmWorkerTypeGroup
                                                     select new FarmWorkerTypesWithCount()
                                                     {
                                                         FarmWorkerTypeDescr = FarmWorkerTypeGroup.Key,
                                                         FarmWorkerCount = FarmWorkerTypeGroup.Count()
                                                     };

                    if (!String.IsNullOrEmpty(""))
                    {
                        farmWorkers = farmWorkers.Where(i => i.FarmWorkerType.FarmWorkerTypeDescr == "");
                        viewModel.FarmWorkerType = "";
                    }

                    //sort results
                    farmWorkers = farmWorkers.OrderBy(i => i.FarmWorkerLName);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.FarmWorkers = farmWorkers.ToPagedList(currentPage, PageItems);


                    FarmWorker fw = new FarmWorker();

                    if (TempData["yay"] != null)
                    {
                        fw = (FarmWorker)TempData["yay"];
                        TempData.Remove("yay");
                    }

                    viewModel.JavaScriptToRun = "mySuccess()";
                    return View("Index", viewModel);
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
                //instatiate new view model
                FarmWorkerIndexViewModel viewModel = new FarmWorkerIndexViewModel();

                //select farmworkers
                var farmWorkers = db.FarmWorkers.Include(f => f.Country).Include(f => f.Farm).Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Province).Include(f => f.Title); ;
                //var farmWorkers = db.FarmWorkers.Include(f => f.FarmWorkerType).Include(f => f.Gender).Include(f => f.Title);

                //do search and save search string in view model
                if (!String.IsNullOrEmpty(""))
                {
                    farmWorkers = farmWorkers.Where(i => i.FarmWorkerFName.Contains("") ||
                        i.FarmWorkerLName.Contains("") || i.FarmWorkerType.FarmWorkerTypeDescr.Contains(""));
                    viewModel.Search = "";
                }

                //group search results into types and count how many workers in each type
                viewModel.WorkerTypesWithCount = from matchingFarmWorkers in farmWorkers
                                                     //where
                                                     //matchingFarmWorkers.FarmWorkerTypeID != null
                                                 group matchingFarmWorkers by
                                                 matchingFarmWorkers.FarmWorkerType.FarmWorkerTypeDescr into
                                                 FarmWorkerTypeGroup
                                                 select new FarmWorkerTypesWithCount()
                                                 {
                                                     FarmWorkerTypeDescr = FarmWorkerTypeGroup.Key,
                                                     FarmWorkerCount = FarmWorkerTypeGroup.Count()
                                                 };

                if (!String.IsNullOrEmpty(""))
                {
                    farmWorkers = farmWorkers.Where(i => i.FarmWorkerType.FarmWorkerTypeDescr == "");
                    viewModel.FarmWorkerType = "";
                }

                //sort results
                farmWorkers = farmWorkers.OrderBy(i => i.FarmWorkerLName);
                //Paging:
                const int PageItems = 5;
                int currentPage = (1);
                viewModel.FarmWorkers = farmWorkers.ToPagedList(currentPage, PageItems);

                ViewData["ErrorMessage"] = "FarmWorker details are used in Attendence Sheets! Cannot delete!";
                ViewBag.Error = "FarmWorker details are used in Attendence Sheets! Cannot delete!";
                TempData["data"] = "FarmWorker details are used in Attendence Sheets! Cannot delete!";

                viewModel.JavaScriptToRun = "myFail()";
                return View("Index", viewModel);

                //return RedirectToAction("Index");
            }

            //return View(farmWorker);
        }

        // POST: FarmWorker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FarmWorker farmWorker = db.FarmWorkers.Find(id);
            db.FarmWorkers.Remove(farmWorker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public bool IsIDExist(string inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.FarmWorkers.Where(a => a.FarmWorkerIDNum == inID).FirstOrDefault();
                return v != null;
            }
        }

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


