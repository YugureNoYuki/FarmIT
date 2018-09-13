using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;

namespace farmLogin.Controllers
{
    public class SupplierOrderController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: SupplierOrder
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Farm).Include(o => o.OrderStatu).Include(o => o.Supplier).Include(o => o.Unit).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: SupplierOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: SupplierOrder/Create
        public ActionResult Create()
        {
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusDescr");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: SupplierOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderNum,OrderDate,OrderItemPrice,SupplierID,UserID,FarmID,OrderStatusID,OrderItem,OrderQty,UnitID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", order.FarmID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusDescr", order.OrderStatusID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", order.SupplierID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", order.UnitID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // GET: SupplierOrder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", order.FarmID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusDescr", order.OrderStatusID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", order.SupplierID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", order.UnitID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // POST: SupplierOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderNum,OrderDate,OrderItemPrice,SupplierID,UserID,FarmID,OrderStatusID,OrderItem,OrderQty,UnitID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FarmID = new SelectList(db.Farms, "FarmID", "FarmName", order.FarmID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "OrderStatusDescr", order.OrderStatusID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", order.SupplierID);
            ViewBag.UnitID = new SelectList(db.Units, "UnitID", "UnitDescr", order.UnitID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // GET: SupplierOrder/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: SupplierOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
