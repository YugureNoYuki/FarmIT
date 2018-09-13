using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(VehicleMakeMetaData))]
    public partial class VehicleMake
    {

    }
    public class VehicleMakeMetaData
    {
        [Key]
        public int VehMakeID { get; set; }

        [Required(ErrorMessage = "Vehicle Make cannot be blank")]
        [Display(Name = "Vehicle Make")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Vehicle Make description must be alphabetic")]
        public string VehMakeDescr { get; set; }
    }
}
