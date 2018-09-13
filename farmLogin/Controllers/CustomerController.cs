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
    public class CustomerController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Customer
        public ActionResult Index(string search, int? page)
        {
            CustomerIndexViewModel viewModel = new CustomerIndexViewModel();
            var customers = db.Customers.Include(c=>c.Country).Include(c=>c.Province);
            if (!String.IsNullOrEmpty(search))
            {
                customers = db.Customers.Where(c => c.CompanyName.Contains(search) ||
                c.CustomerFName.Contains(search) || c.CustomerLName.Contains(search));
                //ViewBag.Search = search;
                viewModel.Search = search;
            }
            //sort results
            customers = customers.OrderBy(i => i.CustomerFName);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.Customers = customers.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr");

            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            Customer customer = new Customer();

            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            return View(customer);
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "CustomerID,CustomerFName,CustomerLName,CustomerContactNum,ContactPersonalEmailAddress,CompanyName,Address,Surburb,City,CountryID,ProvinceID,CompanyTelNum")]*/ Customer customer)
        {

            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            var contactExist = IsContactExist(customer.CustomerContactNum);
            var emailExist = IsEmailExist(customer.ContactPersonalEmailAddress);

            if (contactExist == true && emailExist == true)
            {
                ModelState.AddModelError("NameExist", "Supplier Name already exist");
                ViewBag.Error = "Supplier Name already exists! Please specify different Name.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
                return View(customer);
            }
            else if (emailExist)
            {
                ModelState.AddModelError("EmailExist", "Supplier Email already exist");
                ViewBag.Error = "Supplier Email already exists! Please specify different Email.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
                return View(customer);
            }
            else if (contactExist)
            {
                ModelState.AddModelError("BothExist", "Supplier already exist");
                ViewBag.Error = "Supplier already exists! Please specify different Name & Email.";
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
                return View(customer);
            }

            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                //return RedirectToAction("Index");


                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);

                ////////////////////// ADDED IN CODE /////////////////////////
                customer.JavaScriptToRun = "mySuccess()";
                return View(customer);
            }

            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerFName,CustomerLName,CustomerContactNum,ContactPersonalEmailAddress,CompanyName,Address,Surburb,City,CountryID,ProvinceID,CompanyTelNum")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");

                ////////////////////// ADDED IN CODE /////////////////////////
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
                ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
                customer.JavaScriptToRun = "mySuccess()";
                return View(customer);
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryDescr", customer.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceDescr", customer.ProvinceID);
            return View(customer);
        }


        [NonAction]
        public bool updExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.Customers.Where(a => a.ContactPersonalEmailAddress != inDescr).ToList();

                var v = list.Where(a => a.ContactPersonalEmailAddress == inDescr).FirstOrDefault();
                return v != null;
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
                        //check if notnull and then remove
            if (customer != null && customer.Sales.Count < 1)
            {
                try
                {
                    //remove supplier from db

                    db.Customers.Remove(customer);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    CustomerIndexViewModel viewModel = new CustomerIndexViewModel();
                    var customers = db.Customers.Include(c => c.Country).Include(c => c.Province);
                    if (!String.IsNullOrEmpty(""))
                    {
                        customers = db.Customers.Where(c => c.CompanyName.Contains("") ||
                        c.CustomerFName.Contains("") || c.CustomerLName.Contains(""));
                        //ViewBag.Search = search;
                        viewModel.Search = "";
                    }
                    //sort results
                    customers = customers.OrderBy(i => i.CustomerFName);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.Customers = customers.ToPagedList(currentPage, PageItems);
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

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();




            return View(customer);
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
        public bool IsContactExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Customers.Where(a => a.CustomerContactNum == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool IsEmailExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Customers.Where(a => a.ContactPersonalEmailAddress == inData).FirstOrDefault();
                return v != null;
            }
        }
    }
}
