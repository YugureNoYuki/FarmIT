using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using farmLogin.Models;
using System.Web.Mvc;

namespace farmLogin.ViewModels
{
    public class UserIndexViewModel
    {
        public IPagedList<User> Users { get; set; }
        public string Search { get; set; }

        public string JavaScriptToRun { get; set; }
    }
}