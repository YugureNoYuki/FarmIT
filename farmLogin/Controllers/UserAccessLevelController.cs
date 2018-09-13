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
    public class UserAccessLevelController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: UserAccessLevel
        public ActionResult Index(string search, int? page)
        {
            UserAccessLevelIndexViewModel viewModel = new UserAccessLevelIndexViewModel();
            var useraccesslvls = db.UserAccessLevels.Include(c => c.Users);
            if (!String.IsNullOrEmpty(search))
            {
                useraccesslvls = db.UserAccessLevels.Where(c => c.UserAccessLevelDescr.Contains(search));
                //ViewBag.Search = search;
                viewModel.Search = search;
            }
            //sort results
            useraccesslvls = useraccesslvls.OrderBy(i => i.UserAccessLevelID);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.UserAccessLevels = useraccesslvls.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: UserAccessLevel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevels.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            return View(userAccessLevel);
        }

        // GET: UserAccessLevel/Create
        public ActionResult Create()
        {

            UserAccessLevel ual = new UserAccessLevel();

            if (TempData["yay"] != null)
            {
                ual = (UserAccessLevel)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(ual);
        }

        // POST: UserAccessLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserAccessLevelID,UserAccessLevelDescr,Notes")] UserAccessLevel userAccessLevel)
        {
            var IsExist = IsDescExist(userAccessLevel.UserAccessLevelDescr);
            if (IsExist)
            {
                ModelState.AddModelError("UserAccessLevelExist", "User Access Level already exist");
                ViewBag.Error = "User Access Level already exists! Please specify different Description.";
                return View(userAccessLevel);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges
                        var entity = new UserAccessLevel();

                        entity.UserAccessLevelID = userAccessLevel.UserAccessLevelID;
                        entity.UserAccessLevelDescr = userAccessLevel.UserAccessLevelDescr;
                        //entity.FarmWorkerTypeNotes = userAccessLevel.FarmWorkerTypeNotes;

                        //db.FarmWorkerTypes.Add(entity);

                        db.UserAccessLevels.Add(userAccessLevel);

                        db.SaveChanges();
                        //return RedirectToAction("Index");
                        ViewBag.Success = "Success!";
                        //TempData["UserMessage"] = new MessageVM { CssClassName = "alert-success", Title = "Success!", Message = "Operation Done!" };
                        //return RedirectToAction("Create");


                        //return JavaScript("mySuccess();");
                        //return JavaScript("window.alert('HELLO');");

                        userAccessLevel.JavaScriptToRun = "mySuccess()";
                        return View(userAccessLevel);

                        //return View(inventoryType);
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
            }
            return View(userAccessLevel);
        }

        // GET: UserAccessLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevels.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            return View(userAccessLevel);
        }

        // POST: UserAccessLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserAccessLevelID,UserAccessLevelDescr,Notes")] UserAccessLevel userAccessLevel)
        {
            var IsExist = IsDescExist(userAccessLevel.UserAccessLevelDescr);
            if (IsExist)
            {
                ModelState.AddModelError("UserAccessLevelExist", "User Access Level already exist");
                ViewBag.Error = "User Access Level already exists! Please specify different Description.";
                return View(userAccessLevel);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(userAccessLevel).State = EntityState.Modified;
                    db.SaveChanges();

                    if (TempData["yay"] != null)
                    {
                        userAccessLevel = (UserAccessLevel)TempData["yay"];
                        TempData.Remove("yay");
                    }

                    userAccessLevel.JavaScriptToRun = "mySuccess()";
                    return View(userAccessLevel);
                }
                return View(userAccessLevel);
            }
        }

        // GET: UserAccessLevel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevels.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            //check if notnull and then remove
            if (userAccessLevel != null && userAccessLevel.Users.Count < 1)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.UserAccessLevels.Remove(userAccessLevel);
                    db.SaveChanges();

                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    //instatiate new view model
                    UserAccessLevelIndexViewModel viewModel = new UserAccessLevelIndexViewModel();
                    var useraccesslvls = db.UserAccessLevels.Include(c => c.Users);
                    if (!String.IsNullOrEmpty(""))
                    {
                        useraccesslvls = db.UserAccessLevels.Where(c => c.UserAccessLevelDescr.Contains(""));
                        //ViewBag.Search = search;
                        viewModel.Search = "";
                    }
                    //sort results
                    useraccesslvls = useraccesslvls.OrderBy(i => i.UserAccessLevelID);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.UserAccessLevels = useraccesslvls.ToPagedList(currentPage, PageItems);
                    //return View(viewModel);

                    viewModel.JavaScriptToRun = "myDelSuccess()";
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
            else
            {
                ViewData["ErrorMessage"] = "User Access Level is in use by an User, cannot delete!";
                ViewBag.Error = "User Access Level is in use by an User, cannot delete!";
                TempData["data"] = "User Access Level is in use by an User, cannot delete!";

                var model = db.UserAccessLevels.Select(e => e);

                //instatiate new view model
                UserAccessLevelIndexViewModel viewModel = new UserAccessLevelIndexViewModel();
                var useraccesslvls = db.UserAccessLevels.Include(c => c.Users);
                if (!String.IsNullOrEmpty(""))
                {
                    useraccesslvls = db.UserAccessLevels.Where(c => c.UserAccessLevelDescr.Contains(""));
                    //ViewBag.Search = search;
                    viewModel.Search = "";
                }
                //sort results
                useraccesslvls = useraccesslvls.OrderBy(i => i.UserAccessLevelID);
                //Paging:
                const int PageItems = 5;
                int currentPage = (1);
                viewModel.UserAccessLevels = useraccesslvls.ToPagedList(currentPage, PageItems);
                //return View(viewModel);

                viewModel.JavaScriptToRun = "myFail()";
                return View("Index", viewModel);

                //return RedirectToAction("Index");
            }
        }

        // POST: UserAccessLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccessLevel userAccessLevel = db.UserAccessLevels.Find(id);
            db.UserAccessLevels.Remove(userAccessLevel);
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
        public bool IsDescExist(string inDescr)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.UserAccessLevels.Where(a => a.UserAccessLevelDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }
    }
}
