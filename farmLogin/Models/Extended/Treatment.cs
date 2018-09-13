using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(TreatmentMetaData))]
    public partial class Treatment
    {

    }
    public class TreatmentMetaData
    {
        [Key]
        public int TreatmentID { get; set; }


        [Required(ErrorMessage = "Treatment cannot be blank")]
        [Display(Name = "Treatment")]
        [StringLength(maximumLength: 255, ErrorMessage = "Max 255 characters reached")]
        public string TreatmentDescr { get; set; }
    }
}