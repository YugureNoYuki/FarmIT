using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(VehicleTypeMetaData))]
    public partial class VehicleType
    {
        public string JavaScriptToRun { get; set; }
    }
    public class VehicleTypeMetaData
    {
        [Key]
        public int VehTypeID { get; set; }


        [Required(ErrorMessage = "Vehicle Type cannot be blank")]
        [Display(Name = "Vehicle Type")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Vehicle Type description must be alphabetic")]
        public string VehTypeDescr { get; set; }

        //[Display(Name = "Vehicle Type Image")]
        //[StringLength(maximumLength: 255, ErrorMessage = "Max 255 characters reached")]
        //public string VehTypeImg { get; set; }

    }
}