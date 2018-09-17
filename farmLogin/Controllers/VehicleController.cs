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
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class VehicleController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Vehicle
        public ActionResult Index(string vehicletype, string search, int? page)
        {
            //instatiate new view model
            VehicleIndexViewModel viewModel = new VehicleIndexViewModel();

            //select vehicles
            var vehicles = db.Vehicles.Include(v => v.Farm1).Include(v => v.Unit).Include(v => v.VehicleMake).Include(v => v.VehicleType);

            //do search and save search string in view model
            if (!String.IsNullOrEmpty(search))
            {
                vehicles = vehicles.Where(v => v.VehName.Contains(search) ||
                    v.VehModel.Contains(search) || v.VehicleType.VehTypeDescr.Contains(search));
                viewModel.Search = search;
            }

            //group search results into types and count how many workers in each type
            viewModel.VehTypesWithCount = from matchingVehicles in vehicles
                                              //where
                                              //matchingVehicles.VehicleID != null
                                          group matchingVehicles by
                                          matchingVehicles.VehicleType.VehTypeDescr into
                                          VehicleTypeGroup
                                          select new VehicleTypesWithCount()
                                          {
                                              VehTypeDescr = VehicleTypeGroup.Key,
                                              VehicleCount = VehicleTypeGroup.Count()
                                          };

            if (!String.IsNullOrEmpty(vehicletype))
            {
                vehicles = vehicles.Where(v => v.VehicleType.VehTypeDescr == vehicletype);
                viewModel.VehicleType = vehicletype;
            }

            //sort results
            vehicles = vehicles.OrderBy(i => i.VehicleID);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.Vehicles = vehicles.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName");
            ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS") ), "UnitID", "UnitDescr");
            ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr");
            ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr");

            Vehicle vehicle = new Vehicle();

            return View(vehicle);


        }

        // POST: Vehicle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "VehicleID,VehName,VehYear,VehModel,VehEngineNum,VehVinNum,VehRegNum,VehLicenseNum,VehExpDate,VehCurrMileage,VehServiceInterval,UnitID,VehNextService,Farm,VehTypeID,VehMakeID")]*/ Vehicle vehicle)
        {
            var nameExist = IsNameExist(vehicle.VehName);
            var licenseExist = IsLicenseExist(vehicle.VehLicenseNum);

            if (nameExist == true && licenseExist == true)
            {
                ModelState.AddModelError("VehExist", "Vehicle already exist");
                ViewBag.Error = "Vehicle already exists! Please specify different Name/License Number.";

                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
                return View(vehicle);
            }
            else if (nameExist == true)
            {
                ModelState.AddModelError("VehNameExist", "Vehicle already exist");
                ViewBag.Error = "Vehicle Name already exists! Please specify different Name.";

                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
                return View(vehicle);
            }
            else if(licenseExist == true)
            {
                ModelState.AddModelError("VehLicenseExist", "Vehicle already exist");
                ViewBag.Error = "Vehicle License already exists! Please specify different License Number.";

                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
                return View(vehicle);
            }

            if (ModelState.IsValid)
            {
                vehicle.VehNextService = vehicle.VehCurrMileage + vehicle.VehServiceInterval;
                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);

                vehicle.JavaScriptToRun = "mySuccess()";
                return View(vehicle);


            }

            ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
            ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
            ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
            ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
            ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleID,VehName,VehYear,VehModel,VehEngineNum,VehVinNum,VehRegNum,VehLicenseNum,VehExpDate,VehCurrMileage,VehServiceInterval,UnitID,VehNextService,Farm,VehTypeID,VehMakeID")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(vehicle).State = EntityState.Modified;
                vehicle.VehNextService = vehicle.VehCurrMileage + vehicle.VehServiceInterval;
                db.SaveChanges();
                //return RedirectToAction("Index");

                ////////////////////// ADDED IN CODE /////////////////////////
                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
                vehicle.JavaScriptToRun = "mySuccess()";
                return View(vehicle);

            }
            ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
            ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
            ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }

            //check if notnull and then remove
            if (vehicle != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.Vehicles.Remove(vehicle);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    //instatiate new view model
                    VehicleIndexViewModel viewModel = new VehicleIndexViewModel();

                    //select vehicles
                    var vehicles = db.Vehicles.Include(v => v.Farm1).Include(v => v.Unit).Include(v => v.VehicleMake).Include(v => v.VehicleType);

                    //do search and save search string in view model
                    if (!String.IsNullOrEmpty(""))
                    {
                        vehicles = vehicles.Where(v => v.VehName.Contains("") ||
                            v.VehModel.Contains("") || v.VehicleType.VehTypeDescr.Contains(""));
                        viewModel.Search = "";
                    }

                    //group search results into types and count how many workers in each type
                    viewModel.VehTypesWithCount = from matchingVehicles in vehicles
                                                      //where
                                                      //matchingVehicles.VehicleID != null
                                                  group matchingVehicles by
                                                  matchingVehicles.VehicleType.VehTypeDescr into
                                                  VehicleTypeGroup
                                                  select new VehicleTypesWithCount()
                                                  {
                                                      VehTypeDescr = VehicleTypeGroup.Key,
                                                      VehicleCount = VehicleTypeGroup.Count()
                                                  };

                    if (!String.IsNullOrEmpty(""))
                    {
                        vehicles = vehicles.Where(v => v.VehicleType.VehTypeDescr == "");
                        viewModel.VehicleType = "";
                    }

                    //sort results
                    vehicles = vehicles.OrderBy(i => i.VehicleID);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.Vehicles = vehicles.ToPagedList(currentPage, PageItems);

                    viewModel.JavaScriptToRun = "mySuccess()";
                    return View("Index", viewModel);






                    //return RedirectToAction("Index");
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

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditMileage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            //UpdateMileageModel model = new UpdateMileageModel();
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", vehicle.UnitID);
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMileage([Bind(Include = "VehicleID,VehName,VehYear,VehModel,VehEngineNum,VehVinNum,VehRegNum,VehLicenseNum,VehExpDate,VehCurrMileage,VehServiceInterval,UnitID,VehNextService,Farm,VehTypeID,VehMakeID")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                //vehicle.VehNextService = vehicle.VehCurrMileage + vehicle.VehServiceInterval;
                db.SaveChanges();


                ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
                ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
                ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
                vehicle.JavaScriptToRun = "mySuccess()";
                return View(vehicle);
            }
            ViewBag.Farm = new SelectList(db.Farms, "FarmID", "FarmName", vehicle.Farm);
            ViewBag.UnitID = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("KG") && !u.UnitDescr.Contains("HA") && !u.UnitDescr.Contains("TONNE") && !u.UnitDescr.Contains("GRAMS")), "UnitID", "UnitDescr"); ViewBag.VehMakeID = new SelectList(db.VehicleMakes, "VehMakeID", "VehMakeDescr", vehicle.VehMakeID);
            ViewBag.VehTypeID = new SelectList(db.VehicleTypes, "VehTypeID", "VehTypeDescr", vehicle.VehTypeID);
            vehicle.VehNextService = vehicle.VehCurrMileage + vehicle.VehServiceInterval;
            return View(vehicle);
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
        public bool IsLicenseExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Vehicles.Where(a => a.VehLicenseNum == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool IsNameExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Vehicles.Where(a => a.VehName == inData).FirstOrDefault();
                return v != null;
            }
        }
    }
}
