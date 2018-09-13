using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(VehicleServiceMetaData))]
    public partial class VehicleService
    {
        public string JavaScriptToRun { get; set; }
    }
    public class VehicleServiceMetaData
    {
        [Key]
        public int VehicleServiceID { get; set; }


        [Required(ErrorMessage = "Service Date cannot be blank")]
        [Display(Name = "Service Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime VehicleService_Date { get; set; }

        [Display(Name = "Service Cost")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public Nullable<decimal> VehicleService_Cost { get; set; }

        [Required(ErrorMessage = "Service Record cannot be blank")]
        [Display(Name = "Service Record")]
        [RegularExpression("[0-9]+", ErrorMessage = "Service Record must be numeric")]
        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Service Record cannot be negative or zero")]
        public int VehicleServiceRecord { get; set; }

    }
}