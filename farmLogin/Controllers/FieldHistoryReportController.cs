using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using farmLogin.Models;
using farmLogin.Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace farmLogin.Controllers.Reports
{
    public class FieldHistoryReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        public ActionResult Index()
        {
            var yield = dc.SiloHarvests.Include(y => y.SiloHarvestTonnesStored);
            return View();
        }
    }
}