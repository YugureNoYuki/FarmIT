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
using Microsoft.AspNet.Identity;

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

        [HttpPost]
        public ActionResult RedirectToNotify()
        {
            string url = "/Home/NotifyLicense/";
            return Json(new { Url = url });
        }

        [HttpPost]
        public void NotifyLicense()
        {
            //get list of all vehicles
            var vList = db.Vehicles.ToList();

            //get list of all users
            var uList = db.Users.ToList();

            //get the email of currently logged in email
            var email = User.Identity.GetUserName();

            //user email to find the id of currently logged in user
            var currID = uList.Where(a => a.UserEmailAddress == email).Select(a => a.UserID).FirstOrDefault();

            var fromEmail = new MailAddress("u15185142@tuks.co.za", "FarmIT Account Control"); //Of course I'm awesome
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Pp15185142"; //Password is taken out :)

            foreach (var item in vList)
            {
                var diff = item.VehNextService - item.VehCurrMileage;

                if(diff <= item.VehServiceInterval)
                {
                    string subject = "";
                    string body = "";
                    if (email != null)
                    {
                        subject = "Notify Vehicle Service";

                        body = "<br/><br/>Your Vehicle" + item.VehName + " with License Number of " + item.VehLicenseNum + "Needs to be serviced withing " + diff + "Mileage.";
                    }


                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                    };

                    using (var message = new MailMessage(fromEmail, toEmail)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    })

                        smtp.Send(message);
                }
            }
        }
    }
}