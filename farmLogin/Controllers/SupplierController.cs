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
    public class SupplierController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Supplier
        public ActionResult Index(string suppliername, string search, int? page)
        {
            SupplierIndexViewModel viewModel = new SupplierIndexViewModel();
            //select suppliers
            var suppliers = db.Suppliers.Include(s => s.Country).Include(s => s.Province);
            if (!String.IsNullOrEmpty(search))
            {
                suppliers = suppliers.Where(s => s.SupplierName.Contains(search));
                //ViewBag.Search = search; //store current search in ViewBag
                viewModel.Search = search;
            }

            if (!String.IsNullOrEmpty(suppliername))
            {
                suppliers = suppliers.Where(i => i.SupplierName == suppliername);
                viewModel.SupplierName = suppliername;
            }

            //sort results
            suppliers = suppliers.OrderBy(s => s.SupplierName);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.Suppliers = suppliers.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");

            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            Supplier supplier = new Supplier();

            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            return View(supplier);
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "SupplierID,SupplierName,Address,Surburb,City,CountryID,ProvinceID,SupplierEmailAddress,SupplierPhoneNum,SupplierCellNum")]*/ Supplier supplier)
        {

            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            var nameExist = IsNameExist(supplier.SupplierName);
            var emailExist = IsEmailExist(supplier.SupplierEmailAddress);

            if (nameExist)
            {
                ModelState.AddModelError("NameExist", "Supplier Name already exist");
                ViewBag.Error = "Supplier Name already exists! Please specify different Name.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
                return View(supplier);
            }
            else if (emailExist)
            {
                ModelState.AddModelError("EmailExist", "Supplier Email already exist");
                ViewBag.Error = "Supplier Email already exists! Please specify different Email.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
                return View(supplier);
            }
            else if (nameExist == true && emailExist == true)
            {
                ModelState.AddModelError("BothExist", "Supplier already exist");
                ViewBag.Error = "Supplier already exists! Please specify different Name & Email.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
                return View(supplier);
            }


            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                //return RedirectToAction("Index");

                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
                
                ////////////////////// ADDED IN CODE /////////////////////////
                supplier.JavaScriptToRun = "mySuccess()";
                return View(supplier);

            }

            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,SupplierName,Address,Surburb,City,CountryID,ProvinceID,SupplierEmailAddress,SupplierPhoneNum,SupplierCellNum")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");

                ////////////////////// ADDED IN CODE /////////////////////////
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
                supplier.JavaScriptToRun = "mySuccess()";
                return View(supplier);
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", supplier.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", supplier.ProvinceID);
            return View(supplier);
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            //check if notnull and then remove
            if (supplier != null && supplier.Orders.Count < 1)
            {
                try
                {
                    //remove supplier from db

                    db.Suppliers.Remove(supplier);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    SupplierIndexViewModel viewModel = new SupplierIndexViewModel();
                    //select suppliers
                    var suppliers = db.Suppliers.Include(s => s.Country).Include(s => s.Province);
                    if (!String.IsNullOrEmpty(""))
                    {
                        suppliers = suppliers.Where(s => s.SupplierName.Contains(""));
                        //ViewBag.Search = search; //store current search in ViewBag
                        viewModel.Search = "";
                    }

                    if (!String.IsNullOrEmpty(""))
                    {
                        suppliers = suppliers.Where(i => i.SupplierName == "");
                        viewModel.SupplierName = "";
                    }

                    //sort results
                    suppliers = suppliers.OrderBy(s => s.SupplierName);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.Suppliers = suppliers.ToPagedList(currentPage, PageItems);

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

            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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

        [NonAction]
        public bool IsNameExist(string inName)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Suppliers.Where(a => a.SupplierName == inName).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool IsEmailExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Suppliers.Where(a => a.SupplierEmailAddress == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.Suppliers.Where(a => a.SupplierID == inID).FirstOrDefault();


                var siloList = ctx.Suppliers.Where(a => a.SupplierID != inID).ToList();

                var outList = siloList.Where(a => a.SupplierName != myList.SupplierName);

                var match = outList.Where(a => a.SupplierName == inDescr).FirstOrDefault();

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
