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
    public class VehicleReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        public ActionResult Index()
        {
            var vehicles = dc.Vehicles.Include(v => v.VehicleType).Include(o => o.VehicleServices);
            return View(vehicles.ToList());
        }

        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportVehicles.rpt")));
            rd.SetDataSource(dc.Vehicles.Select(p => new
            {
                Id = p.VehicleID,
                Name = p.VehName,
                Model = p.VehModel,
                EngineNumber = p.VehEngineNum,
                VIN = p.VehVinNum,
                LicenseNumber = p.VehLicenseNum,
                Registration = p.VehRegNum,
                Mileage = p.VehCurrMileage,
                VehicleType = p.VehicleType.VehTypeDescr,
                VehicleMake = p.VehicleMake.VehMakeDescr
            }).ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "VehicleList.pdf");
        }
    }
}