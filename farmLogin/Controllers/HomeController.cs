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
        public void NotifyService()
        {
            //get list of all vehicles
            var vList = db.Vehicles.ToList();

            //get list of all users
            var uList = db.Users.ToList();

            //get the email of currently logged in email
            var email = User.Identity.GetUserName();

            //user email to find the id of currently logged in user
            var currID = uList.Where(a => a.UserEmailAddress == email).Select(a => a.UserID).FirstOrDefault();

            var fromEmail = new MailAddress("u15185142@tuks.co.za", "FarmIT Notification Service"); //Of course I'm awesome
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Pp15185142"; //Password is taken out :)

            foreach (var item in vList)
            {
                var diff = item.VehNextService - item.VehCurrMileage;
                var halfInterval = (item.VehServiceInterval / 2);

                if(diff <= halfInterval)
                {
                    string subject = "";
                    string body = "";
                    if (email != null)
                    {
                        subject = "Notify Vehicle Service";

                        body = "<br/><br/><b>Your Vehicle, " + item.VehName + "<b/>, with License Number of <b>" + item.VehLicenseNum + "<b/> has reached the halfway mark of its Servicing Interval.<br/><br/>Current Mileage: " + item.VehCurrMileage + "<br/>Next Service: " + item.VehNextService + "<br/><br/>Please update the indicated vehicle's servicing details accordingly on the system.<br/><br/>Vehicle's Current Details:<br/><b>Name: <b/>" + item.VehName + "<br/><b>Year: <b/>" + item.VehYear + "<br/><b>Model: <b/>" + item.VehModel + "<br/><b>License Number: <b/>" + item.VehLicenseNum + "<b><br/>Current Mileage: <b/>" + item.VehCurrMileage + "<br/><b>Next Servicing Mileage: <b/>" + item.VehNextService + "<br/><b>Servicing Interval: <b/>" + item.VehServiceInterval + "<br/><br/><b>Disclaimer:<b/> This email will be sent to you everytime you reach the dashboard of the system, until the Vehicle's Servicing Details has been manually updated on the system.";
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

            var fromEmail = new MailAddress("u15185142@tuks.co.za", "FarmIT Notification Service"); //Of course I'm awesome
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Pp15185142"; //Password is taken out :)

            foreach (var item in vList)
            {
                DateTime d1 = DateTime.Now;
                DateTime d2 = item.VehExpDate;

                TimeSpan span = d2.Subtract(d1);
                int diff = (int)span.TotalDays;


                if (diff <= 7)
                {
                    string subject = "";
                    string body = "";
                    if (email != null)
                    {
                        subject = "Notify Vehicle Service";

                        body = "<br/><br/><b>Your Vehicle, " + item.VehName + "<b/>, with License Number of <b>" + item.VehLicenseNum + "<b/> will have its License Number expiring within the next 7 days.<br/><br/>Current Date: " + DateTime.Now.ToShortDateString() + "<br/>License Expiry Date: " + item.VehExpDate.ToShortDateString() + "<br/><br/>Please renew and update the indicated vehicle's license number accordingly on the system.<br/><br/>Vehicle's Current Details:<br/><b>Name: <b/>" + item.VehName + "<br/><b>Year: <b/>" + item.VehYear + "<br/><b>Model: <b/>" + item.VehModel + "<br/><b>License Number: <b/>" + item.VehLicenseNum + "<b><br/>License Expiry Date: " + item.VehExpDate.ToShortDateString() + "<br/><br/>Current Mileage: <b/>" + item.VehCurrMileage + "<br/><b>Next Servicing Mileage: <b/>" + item.VehNextService + "<br/><b>Servicing Interval: <b/>" + item.VehServiceInterval + "<br/><br/><b>Disclaimer:<b/> This email will be sent to you everytime you reach the dashboard of the system, until the Vehicle's Servicing Details has been manually updated on the system.";
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