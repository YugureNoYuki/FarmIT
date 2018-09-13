using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(LandMetaData))]
    public partial class Land
    {
        public string JavaScriptToRun { get; set; }

    }
    public class LandMetaData
    {

        [Key]
        public int LandID { get; set; }

        [Required(ErrorMessage = "Land Name cannot be blank")]
        [Display(Name = "Land Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Land name must be made up of letters and numbers only")]
        public string LandName { get; set; }

    }
}