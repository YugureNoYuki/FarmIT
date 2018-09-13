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
    public class VehicleServiceReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        public ActionResult Index()
        {
            var services = dc.VehicleServices.Include(v => v.Vehicle).Include(v=>v.Unit);
            return View(services.ToList());
        }
        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportVehicleServices.rpt")));
            rd.SetDataSource(dc.VehicleServices.Select(p => new
            {
                Id = p.VehicleServiceID,
                ServiceDate = p.VehicleService_Date,
                ServiceMileage = p.VehicleServiceRecord,
                ServiceCost = p.VehicleService_Cost,
                VehicleID = p.VehicleID,
                VehicleName = p.Vehicle.VehName
            }).ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "VehicleServices.pdf");
        }
    }
}