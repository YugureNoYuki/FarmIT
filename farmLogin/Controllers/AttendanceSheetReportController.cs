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

namespace farmLogin.Controllers
{
    public class AttendanceSheetReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        public ActionResult Index()
        {
            var attendance = dc.AttendenceSheets.Include(a => a.FarmWorker).Include(a => a.User);
            return View(attendance.ToList());
        }

        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportAttendance.rpt")));
            rd.SetDataSource(dc.AttendenceSheets.Select(p => new
            {
                Id = p.AttendenceSheetID,
                ClockInTime = p.ClockInTime,
                ClockOutTime = p.ClockOutTime,
                FarmWorkerNum = p.FarmWorkerNum,
                FarmWorkerName = p.FarmWorker.FarmWorkerFName
            }).ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "AttendanceSheet.pdf");
        }
    }
}