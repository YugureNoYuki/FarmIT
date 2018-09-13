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
    public class InventoryController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }

        // GET: Inventory
        public ActionResult Index(string inventorytype, string search, int? page)
        {
            //instantiate new view model
            InventoryIndexViewModel viewModel = new InventoryIndexViewModel();

            //select inventories
            var inventories = db.Inventories.Include(i => i.InventoryType);

            //do search and save search string to view model
            if (!String.IsNullOrEmpty(search))
            {
                inventories = inventories.Where(i => i.InvDescr.Contains(search) ||
                i.InventoryType.InvTypeDescr.Contains(search));
                //ViewBag.Search = search; //store current search in ViewBag
                viewModel.Search = search;
            }
            /*//generate list of distinct Types to be stored in ViewBag as a SelectList
            var inventorytypes = inventories.OrderBy(i => i.InventoryType.InvTypeDescr).Select(i =>
            i.InventoryType.InvTypeDescr).Distinct();*/

            //group search results into types and count how many items in each type
            viewModel.InvTypesWithCount = from matchingInventories in inventories
                                              //where
                                              //matchingInventories.InvTypeID != null
                                          group matchingInventories by
                                          matchingInventories.InventoryType.InvTypeDescr into
                                          InvTypeGroup
                                          select new InventoryTypesWithCount()
                                          {
                                              InvTypeDescr = InvTypeGroup.Key,
                                              InventoryCount = InvTypeGroup.Count()
                                          };

            if (!String.IsNullOrEmpty(inventorytype))
            {
                inventories = inventories.Where(i => i.InventoryType.InvTypeDescr == inventorytype);
                viewModel.InventoryType = inventorytype;
            }

            inventories = inventories.OrderBy(i => i.InvDescr);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.Inventories = inventories.ToPagedList(currentPage, PageItems);
            return View(viewModel);
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }






        //[NonAction]
        //public bool IsDescExist(string inDescr, string inCode)
        //{
        //    using (FarmDbContext ctx = new FarmDbContext())
        //    {
        //        var v = ctx.Inventories.Where(a => a.InvDescr == inDescr && a.InvCode == inCode).FirstOrDefault();
        //        return v != null;


        //    }
        //}





        // GET: Inventory/Create
        public ActionResult Create()
        {
            ////////////////// ORIGINAL CODE ///////////////////////////
            ////////////////// ORIGINAL CODE ///////////////////////////
            ////////////////// ORIGINAL CODE ///////////////////////////
            
            ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr");
            //ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr");
            ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("HA")), "UnitID", "UnitDescr");


            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////
            /////////////////ADDED IN CODE ///////////////////////////////

            Inventory inv = new Inventory();


            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////
            ///////////////// RETURN TO VIEW ///////////////////////////////

            return View(inv);
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "InventoryID,InvDescr,InvQty,InvDatePurchased,InvCode,Unit,InvTypeID")]*/ InventoryIndexViewModel viewModel, Inventory inventory)
        {

            var descExist = IsDescExist(inventory.InvDescr);
            var codeExist = IsCodeExist(inventory.InvCode);

            if (descExist == true && codeExist == true)
            {
                ModelState.AddModelError("InvTypeExist", "Inventory Type already exist");
                ViewBag.Error = "Inventory Type already exists! Please specify different Description/Code.";

                ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
                ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("HA")), "UnitID", "UnitDescr");
                return View(inventory);
            }


            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();

                ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
                ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("HA")), "UnitID", "UnitDescr");
                
                //instantiate new view model
                //InventoryIndexViewModel viewModel = new InventoryIndexViewModel();



                //viewModel.InventoryID = inventory.InventoryID;
                //viewModel.InvTypeID = inventory.InvTypeID;
                //viewModel.InvDescr = inventory.InvDescr;
                //viewModel.InvQty = inventory.InvQty;
                //viewModel.InvDatePurchased = inventory.InvDatePurchased;
                //viewModel.InvCode = inventory.InvCode;
                //viewModel.Unit = inventory.Unit;

                //viewModel.Inventories = inventory.InventoryType;

                //db.Inventories.Add(viewModel);



                //select inventories
                var inventories = db.Inventories.Include(i => i.InventoryType);




                ////////////////////// ADDED IN CODE /////////////////////////
                inventory.JavaScriptToRun = "mySuccess()";
                return View(inventory);



                //return View("Index", viewModel.Inventories.AsEnumerable());
                //return RedirectToAction("Index");

            }

            ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
            //ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventory.Unit);
            ViewBag.Unit = new SelectList(db.Units.Where(u => !u.UnitDescr.Contains("MM") && !u.UnitDescr.Contains("HOURS") && !u.UnitDescr.Contains("HA")), "UnitID", "UnitDescr");
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventory.Unit);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InventoryID,InvDescr,InvQty,InvDatePurchased,InvCode,Unit,InvTypeID")] Inventory inventory)
        {
            var IsExist = updExist(inventory.InvDescr, inventory.InventoryID);
            if (IsExist)
            {
                ModelState.AddModelError("InvExist", "Inventory Type already exist");
                ViewBag.Error = "Inventory Item already exists! Please specify different Description/Code.";
                ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
                ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventory.Unit);
                //inventory.JavaScriptToRun = "myFail()";
                return View(inventory);
            }

            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");

                ////////////////////// ADDED IN CODE /////////////////////////
                ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
                ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventory.Unit);
                inventory.JavaScriptToRun = "mySuccess()";
                return View(inventory);

            }
            ViewBag.InvTypeID = new SelectList(db.InventoryTypes, "InvTypeID", "InvTypeDescr", inventory.InvTypeID);
            ViewBag.Unit = new SelectList(db.Units, "UnitID", "UnitDescr", inventory.Unit);
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find item to be deleted
            Inventory inventory = db.Inventories.Find(id);

            //chcek fi null
            if (inventory == null)
            {
                return HttpNotFound();
            }

            //check if notnull and then remove
            if(inventory != null && inventory.InventoryTreatments.Count < 1)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.Inventories.Remove(inventory);
                    db.SaveChanges();

                    //var model = db.InventoryTypes.Select(e => e);

                    InventoryIndexViewModel myModel = new InventoryIndexViewModel();
                    myModel.JavaScriptToRun = "mySuccess()";



                    //////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////
                    //InventoryIndexViewModel viewModel = new InventoryIndexViewModel();

                    //select inventories
                    var inventories = db.Inventories.Include(i => i.InventoryType);

                    //do search and save search string to view model
                    if (!String.IsNullOrEmpty(""))
                    {
                        inventories = inventories.Where(i => i.InvDescr.Contains("") ||
                        i.InventoryType.InvTypeDescr.Contains(""));
                        //ViewBag.Search = search; //store current search in ViewBag
                        myModel.Search = "";
                    }
                    /*//generate list of distinct Types to be stored in ViewBag as a SelectList
                    var inventorytypes = inventories.OrderBy(i => i.InventoryType.InvTypeDescr).Select(i =>
                    i.InventoryType.InvTypeDescr).Distinct();*/

                    //group search results into types and count how many items in each type
                    myModel.InvTypesWithCount = from matchingInventories in inventories
                                                      //where
                                                      //matchingInventories.InvTypeID != null
                                                  group matchingInventories by
                                                  matchingInventories.InventoryType.InvTypeDescr into
                                                  InvTypeGroup
                                                  select new InventoryTypesWithCount()
                                                  {
                                                      InvTypeDescr = InvTypeGroup.Key,
                                                      InventoryCount = InvTypeGroup.Count()
                                                  };

                    if (!String.IsNullOrEmpty(""))
                    {
                        inventories = inventories.Where(i => i.InventoryType.InvTypeDescr == "");
                        myModel.InventoryType = "";
                    }

                    inventories = inventories.OrderBy(i => i.InvDescr);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    myModel.Inventories = inventories.ToPagedList(currentPage, PageItems);
                    return View("Index", myModel);






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
                inventory.JavaScriptToRun = "myFail()";
                return View("Index", inventory);

            }
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
                var v = ctx.Inventories.Where(a => a.InvDescr == inDescr).FirstOrDefault();
                return v != null;
            }
        }


        [NonAction]
        public bool IsCodeExist(string inCode)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var v = ctx.Inventories.Where(a => a.InvCode == inCode).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool updExist(string inDescr, int inID)
        {
            using (FarmDbContext ctx = new FarmDbContext())
            {
                var myList = ctx.Inventories.Where(a => a.InventoryID == inID).FirstOrDefault();


                var excList = ctx.Inventories.Where(a => a.InventoryID != inID).ToList();

                var outList = excList.Where(a => a.InvDescr != myList.InvDescr);

                var match = outList.Where(a => a.InvDescr == inDescr).FirstOrDefault();

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
