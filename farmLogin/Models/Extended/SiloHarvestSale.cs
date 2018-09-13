using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(SiloHarvestSaleMetaData))]
    public partial class SiloHarvestSale
    {

    }
    public class SiloHarvestSaleMetaData
    {
        [Key, Column(Order = 0)]
        public int SiloHarvestID { get; set; }
        [Key, Column(Order = 1)]
        public int SaleID { get; set; }

        [Required(ErrorMessage = "Harvest Sale Total Amount cannot be blank")]
        [Display(Name = "Harvest Sale Total Amount")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal SiloHarvestSaleTotalAmnt { get; set; }

        [Required(ErrorMessage = "Harvest Sale Total Quantity cannot be blank")]
        [Display(Name = "Harvest Sale Total Quantity")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        //[Range(minimum: 1, maximum: 999999, ErrorMessage = "Quantity cannot be negative or zero")]
        public decimal SiloHarvestSaleTotalQty { get; set; }
    }
}