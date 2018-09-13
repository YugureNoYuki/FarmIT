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
    public class UserController : Controller
    {
        FarmDbContext db = new FarmDbContext();

        public ActionResult ActionPage()
        {
            return View();
        }
        public ActionResult Index(string search, int? page)
        {
            UserIndexViewModel viewModel = new UserIndexViewModel();
            var users = db.Users.Include(v=>v.UserAccessLevel);
            if (!String.IsNullOrEmpty(search))
            {
                users = db.Users.Where(u => u.UserFName.Contains(search) ||
                u.UserLName.Contains(search));
                //ViewBag.Search = search;
                viewModel.Search = search;
            }
            //sort results
            users = users.OrderBy(u => u.UserID);
            //Paging:
            const int PageItems = 5;
            int currentPage = (page ?? 1);
            viewModel.Users = users.ToPagedList(currentPage, PageItems);

            User user = new User();

            if (TempData["yay"] != null)
            {
                user = (User)TempData["yay"];
                TempData.Remove("yay");
            }

            return View(viewModel);
            //var users = db.Users.Include(u => u.UserAccessLevel);
            //return View(users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }
        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserAccessLevelID = new SelectList(db.UserAccessLevels, "UserAccessLevelID", "UserAccessLevelDescr", user.UserAccessLevelID);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "IsEmailValid,ActivationCode,UserPassword,UserName,UserEmailAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(user).State = EntityState.Modified;

                User u = new User();
                u.UserID = user.UserID;
                u.UserIDNum = user.UserIDNum;
                u.UserLName = user.UserFName;
                u.UserLName = user.UserLName;
                u.UserEmailAddress = user.UserEmailAddress;
                u.UserContactNum = user.UserContactNum;
                u.UserAccessLevelID = user.UserAccessLevelID;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserAccessLevelID = new SelectList(db.UserAccessLevels, "UserAccessLevelID", "UserAccessLevelDescr", user.UserAccessLevelID);
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.AttendenceSheets.Count < 1 && user != null)
            {
                try
                {
                    //db.InventoryTypes.De

                    db.Users.Remove(user);
                    //user.FarmWorkerTypeActiveStatus = "ARCHIVED";

                    db.SaveChanges();

                    var model = db.VehicleTypes.Select(e => e);
                    //var myModel = model.ToList();
                    //myModel.

                    //InventoryIndexViewModel myModel = new InventoryIndexViewModel();

                    //user.JavaScriptToRun = "myDelSuccess()";
                    ////TempData["success"] = "success";
                    //return View("Index", user);

                    UserIndexViewModel viewModel = new UserIndexViewModel();
                    var users = db.Users.Include(v => v.UserAccessLevel);
                    if (!String.IsNullOrEmpty(""))
                    {
                        users = db.Users.Where(u => u.UserFName.Contains("") ||
                        u.UserLName.Contains(""));
                        //ViewBag.Search = search;
                        viewModel.Search = "";
                    }
                    //sort results
                    users = users.OrderBy(u => u.UserID);
                    //Paging:
                    const int PageItems = 5;
                    int currentPage = (1);
                    viewModel.Users = users.ToPagedList(currentPage, PageItems);

                    //User user = new User();

                    if (TempData["yay"] != null)
                    {
                        user = (User)TempData["yay"];
                        TempData.Remove("yay");
                    }

                    viewModel.JavaScriptToRun = "myDelSuccess()";
                    return View("Index", viewModel);


                    //return RedirectToAction("Index", inventoryType);
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
                ViewData["ErrorMessage"] = "User Access Level is in use by a User, cannot delete!";
                ViewBag.Error = "User Access Level is in use by a User, cannot delete!";
                TempData["data"] = "Vehicle TypeUser Access Level is in use by a User, cannot delete!";

                var model = db.Users.Select(e => e);

                //user.JavaScriptToRun = "myFail()";
                //return View("Index", User);


                UserIndexViewModel viewModel = new UserIndexViewModel();
                var users = db.Users.Include(v => v.UserAccessLevel);
                if (!String.IsNullOrEmpty(""))
                {
                    users = db.Users.Where(u => u.UserFName.Contains("") ||
                    u.UserLName.Contains(""));
                    //ViewBag.Search = search;
                    viewModel.Search = "";
                }
                //sort results
                users = users.OrderBy(u => u.UserID);
                //Paging:
                const int PageItems = 5;
                int currentPage = (1);
                viewModel.Users = users.ToPagedList(currentPage, PageItems);

                //User user = new User();

                if (TempData["yay"] != null)
                {
                    user = (User)TempData["yay"];
                    TempData.Remove("yay");
                }

                viewModel.JavaScriptToRun = "myFail()";
                return View("Index", viewModel);


                //return RedirectToAction("Index");
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            User user = new User();


            if (TempData["yay"] != null)
            {
                user = (User)TempData["yay"];
                TempData.Remove("yay");
            }
            ViewBag.UserAccessLevelID = new SelectList(db.UserAccessLevels, "UserAccessLevelID", "UserAccessLevelDescr");
            return View(user);

            
            //return View();
        }
        //Registration POST Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailValid,ActivationCode")]User user)
        {
            bool Status = false;
            string message = "";

            // Model Validation
            if (ModelState.IsValid)
            {
                #region Email is already Exist
                var IsExist = IsEmailExist(user.UserEmailAddress);
                if (IsExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Generate Password
                user.UserPassword = RandomString(6);
                //Send Email to User
                SendVerificationLinkEmail(user.UserEmailAddress, user.UserPassword, user.ActivationCode.ToString());
                #endregion

                #region Password Hashing
                user.UserPassword = Crypto.Hash(user.UserPassword);
                //user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save to Database
                using (FarmDbContext dc = new FarmDbContext())
                {
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    //SendVerificationLinkEmail(user.UserEmailAddress,user.UserPassword,user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link has been sent to " + user.UserEmailAddress;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            ViewBag.UserAccessLevelID = new SelectList(db.UserAccessLevels, "UserAccessLevelID", "UserAccessLevelDescr", user.UserAccessLevelID);
            user.JavaScriptToRun = "mySuccess()";
            return View(user);
        }
        //Password generate
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (FarmDbContext dc = new FarmDbContext())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //Avoid confirm password does not match issue
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string message = "";
            using (FarmDbContext dc = new FarmDbContext())
            {
                var v = dc.Users.Where(a => a.UserEmailAddress == login.UserEmailAddress).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.UserPassword), v.UserPassword) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.UserEmailAddress, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            //return Redirect(ReturnUrl);
                            return View("Login", "User");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid Username or Password";
                    }
                }
                else
                {
                    message = "Invalid Username or Password";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (FarmDbContext dc = new FarmDbContext())
            {
                var v = dc.Users.Where(a => a.UserEmailAddress == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email,string password,string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("u15185142@tuks.co.za", "FarmIT Account Control"); //Of course I'm awesome
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Pp15185142"; //Password is taken out :)

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";

                body = "<br/><br/>We are excited to tell you that your FarmIT account has successfully been created. Please click on the link below to verify your account" +
                "<br/><br/><a href='" + link + "'>" + link + "</a>" +
                "<br/><br/>Your temporary password:<b>"+password+ "</b><br/><br/>";
            } 
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got a request to reset your account password. Please click <a href='" + link + "'>here</a> to rest your password. <br/> ";
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

        //Forgot Password? //

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            //Verify Email
            //Generate Reset password Link
            //Send Email
            string message = "";
            bool status = false;

            using (FarmDbContext dc = new FarmDbContext())
            {
                var account = dc.Users.Where(a => a.UserEmailAddress == email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.UserEmailAddress,"",resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false; //Avoid password does not match issue
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email";
                    TempData["yay"] = "Password has successfully been Reset!";
                    return View();
                }
                else
                {
                    message = "Account not found";
                    TempData["fail"] = "Email could not be found, please enter a valid email address!";
                    return View();
                }
            }
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Email account associated with the link
            //Redirect to reset password page
            using (FarmDbContext dc = new FarmDbContext())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (FarmDbContext dc = new FarmDbContext())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.UserPassword = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = ""; //user can only update password once with given resetcode
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        TempData["yay"] = "New password updated successfully!";
                        return View();
                    }
                    else
                    {
                        TempData["fail"] = "Password could not be updated";
                        return View();
                    }
                }
            }
            return View();
        }
    }

}