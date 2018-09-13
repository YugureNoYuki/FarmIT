using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace farmLogin.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult ActionPage()
        {
            return View();
        }


        // GET: Report
        public ActionResult LandRainfall()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport3.rpt"));
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "RainfallLand.pdf");
            }
            catch
            {
                throw;
            }
        }
    }
}