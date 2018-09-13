using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(InventoryTreatmentMetaData))]
    public partial class InventoryTreatment
    {
        public string JavaScriptToRun { get; set; }
    }
    public class InventoryTreatmentMetaData
    {
        [Key, Column(Order =0)]
        public int TreatmentID { get; set; }
        [Key, Column(Order =1)]
        public int InventoryID { get; set; }


        [Required(ErrorMessage = "Quantity cannot be blank")]
        [Display(Name = "Quantity")]
        [RegularExpression("[0-9]+", ErrorMessage = "Quantity must be numeric")]
        [Range(minimum: 1, maximum: 999999, ErrorMessage = "Quantity cannot be negative or zero")]
        public int TreatmentQnty { get; set; }


    }
}