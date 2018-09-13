using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(SaleMetaData))]
    public partial class Sale
    {

    }
    public class SaleMetaData
    {
        [Key]
        public int SaleID { get; set; }

        [Required(ErrorMessage = "Sale Date cannot be blank")]
        [Display(Name = "Sale Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "Quantity cannot be blank")]
        [Display(Name = "Quantity")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Quantity must be numeric")]
        //[Range(minimum: 1, maximum: 999999, ErrorMessage = "Quantity cannot be negative or zero")]
        public decimal SaleQty { get; set; }

        [Required(ErrorMessage = "Sale Amount cannot be blank")]
        [Display(Name = "Sale Amount")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal SaleAmnt { get; set; }

        [Required(ErrorMessage = "Purchase Agreement cannot be blank. Provide N/A if not applicable")]
        [Display(Name = "Purchase Agreement")]
        [StringLength(maximumLength: 255, ErrorMessage = "Max 255 characters reached")]
        [DataType(DataType.MultilineText)]
        public string PurchaseAgreement { get; set; }
    }
}