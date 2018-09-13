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
    public class NaturalDisasterController : Controller
    {
        private FarmDbContext db = new FarmDbContext();

        // GET: NaturalDisaster
        public ActionResult Index()
        {
            return View(db.NaturalDisasters.ToList());
        }

        // GET: NaturalDisaster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaturalDisaster naturalDisaster = db.NaturalDisasters.Find(id);
            if (naturalDisaster == null)
            {
                return HttpNotFound();
            }
            return View(naturalDisaster);
        }
    }
}
