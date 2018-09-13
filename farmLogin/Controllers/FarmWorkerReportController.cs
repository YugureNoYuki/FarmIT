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
    public class FarmWorkerReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        public ActionResult Index()
        {
            var farmworker = dc.FarmWorkers.Include(o => o.Title).Include(o => o.FarmWorkerType).Include(o => o.Farm).Include(o => o.Province).Include(o => o.Country);
            return View(farmworker.ToList());
        }
        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportFarmWorkers.rpt")));
            rd.SetDataSource(dc.FarmWorkers.Select(p => new
            {
                Id = p.FarmWorkerNum,
                FirstName = p.FarmWorkerFName,
                LastName = p.FarmWorkerLName,
                IDNumber = p.FarmWorkerIDNum,
                ContactNumber = p.FarmWorkerContactNum,
                Title = p.Title.TitleDescr,
                Gender = p.Gender.GenderDescr,
                ContractStartDate = p.ContractStartDate,
                ContractEndDate = p.ContractEndDate,
                FarmWorkerType = p.FarmWorkerType.FarmWorkerTypeDescr
            }).ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "FarmWorkerList.pdf");
        }
    }
}