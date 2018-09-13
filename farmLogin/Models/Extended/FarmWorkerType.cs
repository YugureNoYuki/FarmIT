using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(FarmWorkerTypeMetaData))]
    public partial class FarmWorkerType
    {
        public string JavaScriptToRun { get; set; }
    }
    public class FarmWorkerTypeMetaData
    {
        [Key]
        public int FarmWorkerTypeID { get; set; }

        [Required(ErrorMessage = "Farm Worker Type cannot be blank")]
        [Display(Name = "Farm Worker Type")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Farm Worker Type description must be alphabetic")]
        public string FarmWorkerTypeDescr { get; set; }

        [Display(Name = "Notes")]
        [StringLength(maximumLength: 50, ErrorMessage = "Max 50 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Farm Worker Type description must be alphabetic")]
        [DataType(DataType.MultilineText)]
        public string FarmWorkerTypeNotes { get; set; }

        
        [Display(Name = "Active Status")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        public bool FarmWorkerTypeActiveStatus { get; set; }

    }
}