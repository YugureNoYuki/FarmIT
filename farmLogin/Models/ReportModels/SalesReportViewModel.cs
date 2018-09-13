using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    public class SalesReportViewModel
    {
        [DataType(DataType.Date)]
        public System.DateTime SaleDate { get; set; }
        public string CompanyName { get; set; }
        public string CropTypeDescr { get; set; }
        public double SiloHarvestSaleTotalAmnt { get; set; }
        public double SaleAmnt { get; set; }
    }
}