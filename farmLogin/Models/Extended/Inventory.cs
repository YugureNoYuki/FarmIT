using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(InventoryMetaData))]
    public partial class Inventory
    {
        public string JavaScriptToRun { get; set; }
    }
    public class InventoryMetaData
    {

        [Key]
        public int InventoryID { get; set; }


        [Required(ErrorMessage = "Description cannot be blank")]
        [Display(Name = "Description")]
        [StringLength(maximumLength:35, ErrorMessage ="Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage ="Description must be made up of letters and numbers only")]
        public string InvDescr { get; set; }

        [Required(ErrorMessage = "Quantity cannot be blank")]
        [Display(Name = "Quantity")]
        [RegularExpression("[0-9]+", ErrorMessage = "Quantity must be numeric")]
        [Range(minimum:1,maximum:999999, ErrorMessage ="Quantity cannot be negative")]
        public int InvQty { get; set; }

        [Required(ErrorMessage = "Purchase date cannot be blank")]
        [Display(Name = "Date Purchased")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Date must be before or on Today")]
        public System.DateTime InvDatePurchased { get; set; }

        [Required(ErrorMessage = "Inventory Code cannot be blank")]
        [Display(Name = "Inventory Code")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Inventory Code must be made up of letters and numbers only")]
        public string InvCode { get; set; }

        //public string JavaScriptToRun { get; set; }
    }


}
