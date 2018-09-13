using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(FarmMetaData))]
    public partial class Farm
    {
        public string JavaScriptToRun { get; set; }
    }
    public partial class FarmMetaData
    {
        [Key]
        public int FarmID { get; set; }

        [Required(ErrorMessage = "Farm Name cannot be blank")]
        [Display(Name = "Farm Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Farm Name must be made up of letters and numbers only")]
        public string FarmName { get; set; }
    }
}