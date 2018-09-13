using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using System.Data.Entity.Validation;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Controllers
{
    public class FieldController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: Field
        public ActionResult Index()
        {
            var fields = new Field();
            return View(fields);
        }

        // GET: Field/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // GET: Field/Create
        public ActionResult Create()
        {
            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr");
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName");
            Field field = new Field();
            return View(field);
        }

        // POST: Field/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FieldID,FieldName,FieldHectares,FieldStatusID,LandID")] Field field)
        {
            var nameExist = IsNameExist(field.FieldName);
            if (nameExist)
            {
                ModelState.AddModelError("FieldExist", "Field already exist");
                ViewBag.Error = "Field already exists! Please specify different Field Name.";
                ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
                ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
                return View(field);
            }
            if (ModelState.IsValid)
            {
                db.Fields.Add(field);
                db.SaveChanges();
                field.JavaScriptToRun = "mySuccess()";
                ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
                ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
                return View(field);
            }

            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
            return View(field);
        }

        // GET: Field/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
            return View(field);
        }

        // POST: Field/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FieldID,FieldName,FieldHectares,FieldStatusID,LandID")] Field field)
        {
            var IsExist = updExist(field.FieldName);
            if (IsExist)
            {
                ModelState.AddModelError("FieldExist", "Field already exists, please specify different Field Name.");
                ViewBag.Error = "Field already exists! Please specify different Field Name.";
                return View(field);
            }

            if (ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
                ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
                field.JavaScriptToRun = "mySuccess()";
                return View(field);
            }
            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
            return View(field);
        }

        // GET: Field/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            if (field.Plantations.Count < 1 && field != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.Fields.Remove(field);

                    db.SaveChanges();

                    //field.JavaScriptToRun = "myDelSuccess()";
                    //return View("Index", field);

                    var farms = new Farm();
                    farms.JavaScriptToRun = "myDelSuccess()";
                    return View("../Farm/Index", farms);
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
                ViewData["ErrorMessage"] = "Field cannot be deleted! Field is linked to Plantation in system!";
                ViewBag.Error = "Field cannot be deleted! Field is linked to Plantation in system!";
                TempData["data"] = "Field cannot be deleted! Field is linked to Plantation in system!";

                //field.JavaScriptToRun = "myFieldFail()";
                //return View("Index", field);
                var farms = new Farm();
                farms.JavaScriptToRun = "myFieldFail()";
                return View("../Farm/Index", farms);
            }
        }

        // POST: Field/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
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



        public ActionResult SetStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
            return View(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetStatus(Field field)
        {
            if (ModelState.IsValid)
            {
                //double hec = Convert.ToDouble(field.FieldHectares);
                //field.FieldHectares = hec;
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
                //ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
                field.JavaScriptToRun = "mySuccess()";
                return View(field);
            }
            ViewBag.FieldStatusID = new SelectList(db.FieldStatus, "FieldStatID", "FieldStatDescr", field.FieldStatusID);
            ViewBag.LandID = new SelectList(db.Lands, "LandID", "LandName", field.LandID);
            return View(field);
        }
        //VERSION 1

        //public class DecimalModelBinder : IModelBinder
        //{
        //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //    {
        //        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //        if (string.IsNullOrEmpty(valueResult.AttemptedValue))
        //        {
        //            return 0m;
        //        }
        //        var modelState = new ModelState { Value = valueResult };
        //        object actualValue = null;
        //        try
        //        {
        //            actualValue = Convert.ToDecimal(
        //                valueResult.AttemptedValue.Replace(",", "."),
        //                CultureInfo.InvariantCulture
        //            );
        //        }
        //        catch (FormatException e)
        //        {
        //            modelState.Errors.Add(e);
        //        }

        //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
        //        return actualValue;
        //    }
        //}

        //VERSION 2
        //public class DecimalModelBinder : IModelBinder
        //{
        //    public object BindModel(ControllerContext controllerContext,
        //        ModelBindingContext bindingContext)
        //    {
        //        ValueProviderResult valueResult = bindingContext.ValueProvider
        //            .GetValue(bindingContext.ModelName);
        //        ModelState modelState = new ModelState { Value = valueResult };
        //        object actualValue = null;
        //        try
        //        {
        //            actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
        //                CultureInfo.CurrentCulture);
        //        }
        //        catch (FormatException e)
        //        {
        //            modelState.Errors.Add(e);
        //        }

        //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
        //        return actualValue;
        //    }
        //}

        //public class DecimalAttribute : RegularExpressionAttribute
        //{
        //    public int DecimalPlaces { get; set; }
        //    public DecimalAttribute(int decimalPlaces)
        //        : base(string.Format(@"^\d*\.?\d{{0,{0}}}$", decimalPlaces))
        //    {
        //        DecimalPlaces = decimalPlaces;
        //    }

        //    public override string FormatErrorMessage(string name)
        //    {
        //        return string.Format("This number can have maximum {0} decimal places", DecimalPlaces);
        //    }
        //}
        [NonAction]
        public bool IsNameExist(string inData)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Fields.Where(a => a.FieldName == inData).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var list = ctx.Fields.Where(a => a.FieldName != inDescr).ToList();

                var v = list.Where(a => a.FieldName == inDescr).FirstOrDefault();
                return v != null;
            }
        }
    }
}
