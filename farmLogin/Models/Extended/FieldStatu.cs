using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(FieldStatusMetaData))]
    public partial class FieldStatu
    {

    }
    public class FieldStatusMetaData
    {

        [Key]
        public int FieldStatID { get; set; }


        [Required(ErrorMessage = "Field Status cannot be blank")]
        [Display(Name = "Field Status")]
        [StringLength(maximumLength: 25, ErrorMessage = "Max 25 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Field Status description must be alphabetic")]
        public string FieldStatDescr { get; set; }

        [Required(ErrorMessage = "Status Pre-Condition cannot be blank")]
        [Display(Name = "Status Pre-Condition")]
        [StringLength(maximumLength: 255, ErrorMessage = "Max 255 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Status Pre-Condition must be alphabetic")]
        public string StatPreConditions { get; set; }
    }
}