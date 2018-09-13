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
using System.Web.Security;
using System.Net.Mail;
using System.Data.Entity.Validation;

namespace farmLogin.Controllers
{
    public class HomeController : Controller
    {
        FarmDbContext db = new FarmDbContext();

        
        public HomeController()
        {
            db = new FarmDbContext(); 
        }

        [Authorize]
        public ActionResult Index()
        {
            using (FarmDbContext db = new FarmDbContext())
            {
                //Pie Chart
                var list = db.Fields.ToList();
                List<int> repartitions = new List<int>();
                var FieldStatus = list.Select(f => f.FieldStatu.FieldStatID).Distinct();

                foreach (var item in FieldStatus)
                {
                    repartitions.Add(list.Count(f => f.FieldStatu.FieldStatID == item));
                }

                var rep = repartitions;
                ViewBag.FIELDS = FieldStatus;
                ViewBag.REP = repartitions.ToList();

                //Bar Chart

                var barList = db.SiloHarvests.ToList(); // Make a list of everything in SiloHarvest Table
                List<double> repartitionsBar = new List<double>();
                var Silo = barList.Select(a => a.SiloID).Distinct().ToList(); //List the Different Categories of Silos
                var SiloList = db.Silos.Distinct().ToList();
                IQueryable<object> q = db.Silos.Select(b => b.SiloDescr);
                List<object> SiloName = q.ToList();


                foreach (var IT in SiloList)
                {
                    var HarvestList = db.SiloHarvests.Where(y => y.SiloID == IT.SiloID);
                    double Total = 0;
                    foreach (var x in HarvestList)
                    {
                        int harv = Convert.ToInt32(x.SiloHarvestTonnesStored);
                        Total += harv;
                    }
                    repartitionsBar.Add(Total);
                }
                
                var repBar = repartitionsBar;
                ViewBag.SILOS = SiloName;
                ViewBag.REPBAR = repartitionsBar.ToList();

                //Silo Capacity and Crop Type
                var silos = db.Silos.Include(s => s.Unit);


                return View(silos.ToList());

            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize (Roles ="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }
        [Authorize (Roles = "User")]
        public ActionResult UserIndex()
        {
            return View();
        }
    }
}