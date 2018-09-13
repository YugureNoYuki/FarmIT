using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using farmLogin.Models;
using PagedList;
using System.Web.Mvc;

namespace farmLogin.ViewModels
{
    public class UserAccessLevelIndexViewModel
    {
        public IPagedList<UserAccessLevel> UserAccessLevels { get; set; }
        public string Search { get; set; }

        public string JavaScriptToRun { get; set; }
    }
}