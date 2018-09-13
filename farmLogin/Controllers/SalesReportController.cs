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
    public class SalesReportController : Controller
    {
        FarmDbContext dc = new FarmDbContext();
        SalesReportViewModel s = new SalesReportViewModel();
        public ActionResult Index()
        {
            using (FarmDbContext dc = new FarmDbContext())
            {
                List<SalesReportViewModel> model = new List<SalesReportViewModel>();
                var newData = (from a in dc.Customers
                               join b in dc.Sales on a.CustomerID equals b.CustomerID
                               join c in dc.SiloHarvestSales on b.SaleID equals c.SaleID
                               join d in dc.SiloHarvests on c.SiloHarvestID equals d.SiloHarvestID
                               join e in dc.Plantations on d.PlantationID equals e.PlantationID
                               join f in dc.CropTypes on e.CropTypeID equals f.CropTypeID

                               orderby b.SaleDate
                               select new SalesReportViewModel
                               {
                                   SaleDate = b.SaleDate,
                                   CompanyName = a.CompanyName,
                                   CropTypeDescr = f.CropTypeDescr,
                                   SiloHarvestSaleTotalAmnt = (double)c.SiloHarvestSaleTotalAmnt,
                                   SaleAmnt = (double)b.SaleAmnt
                                   //SaleDate = b.SaleDate,
                                   //CompanyName = a.CompanyName,
                                   //CropTypeDescr = f.CropTypeDescr,
                                   //SiloHarvestSaleTotalQty = c.SiloHarvestSaleTotalAmnt,
                                   //SaleAmount = b.SaleAmnt
                               }).ToList();
                //model.Add();
                //foreach (var item in newData)
                //{
                //    s.CompanyName = item.CompanyName;
                //    s.CropTypeDescr = item.CropTypeDescr;
                //    s.SaleAmnt = item.SaleAmnt;
                //    s.SaleDate = item.SaleDate;
                //    s.SiloHarvestSaleTotalAmnt = item.SiloHarvestSaleTotalAmnt;

                //    model.Add(s);
                //}
                    //ViewBag.newDate = newData;
                return View(newData);
                }
                //SalesReportViewModel model = new SalesReportViewModel
                //{
                //    SaleDate = 
                //};
                //return View(newData);
        }

        public ActionResult Export()
        {
            var newData = (from a in dc.Customers
                           join b in dc.Sales on a.CustomerID equals b.CustomerID
                           join c in dc.SiloHarvestSales on b.SaleID equals c.SaleID
                           join d in dc.SiloHarvests on c.SiloHarvestID equals d.SiloHarvestID
                           join e in dc.Plantations on d.PlantationID equals e.PlantationID
                           join f in dc.CropTypes on e.CropTypeID equals f.CropTypeID

                           orderby b.SaleDate
                           select new SalesReportViewModel
                           {
                               SaleDate = b.SaleDate,
                               CompanyName = a.CompanyName,
                               CropTypeDescr = f.CropTypeDescr,
                               SiloHarvestSaleTotalAmnt = (double)c.SiloHarvestSaleTotalAmnt,
                               SaleAmnt = (double)b.SaleAmnt
                               //SaleDate = b.SaleDate,
                               //CompanyName = a.CompanyName,
                               //CropTypeDescr = f.CropTypeDescr,
                               //SiloHarvestSaleTotalQty = c.SiloHarvestSaleTotalAmnt,
                               //SaleAmount = b.SaleAmnt
                           }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportSalesReport.rpt")));
            //rd.SetDataSource(dc.FarmWorkers.Select(p => new
            //{
            //    Id = p.FarmWorkerNum,
            //    FirstName = p.FarmWorkerFName,
            //    LastName = p.FarmWorkerLName,
            //    IDNumber = p.FarmWorkerIDNum,
            //    ContactNumber = p.FarmWorkerContactNum,
            //    Title = p.Title.TitleDescr,
            //    Gender = p.Gender.GenderDescr,
            //    ContractStartDate = p.ContractStartDate,
            //    ContractEndDate = p.ContractEndDate,
            //    FarmWorkerType = p.FarmWorkerType.FarmWorkerTypeDescr
            //}).ToList());
            SalesReportViewModel model = new SalesReportViewModel();
            rd.SetDataSource(newData.Select(p => new
            {
                SaleDate = p.SaleDate,
                CompanyName = p.CompanyName,
                CropTypeDescr = p.CropTypeDescr,
                SiloHarvestSaleTotalAmnt = p.SiloHarvestSaleTotalAmnt,
                SaleAmnt = p.SaleAmnt
            }
            ));

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "SalesList.pdf");
        }
    }
}