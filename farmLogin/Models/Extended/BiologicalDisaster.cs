using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(BiologicalDisasterMetaData))]
    public partial class BiologicalDisaster
    {
        public string JavaScriptToRun { get; set; }
    }
    public class BiologicalDisasterMetaData
    {
        [Key]
        public int BioDisasterID { get; set; }

        [Required(ErrorMessage = "Biological Disaster cannot be blank")]
        [Display(Name = "Biological Disaster")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Biological Disaster must be alphabetic")]
        public string BioDisasterDescr { get; set; }

        [Display(Name = "Biological Disaster Notes")]
        [StringLength(maximumLength: 50, ErrorMessage = "Max 50 characters reached")]
        [DataType(DataType.MultilineText)]
        public string BioDisasterNotes { get; set; }
    }
}