using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(CropCycleMetaData))]
    public partial class CropCycle
    {
        public string JavaScriptToRun { get; set; }
    }
    public class CropCycleMetaData
    {
        [Key]
        public int CropCycleID { get; set; }

        [Required(ErrorMessage = "Start Date cannot be blank")]
        [Display(Name = "Start Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime CropCycleStartDate { get; set; }

        [Required(ErrorMessage = "End Date cannot be blank")]
        [Display(Name = "End Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime CropCycleEndDate { get; set; }

        [Required(ErrorMessage = "Crop Cycle Description cannot be blank")]
        [Display(Name = "Crop Cycle")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        //[RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Crop Cycle description must be alphabetic")]
        public string CropCycleDescr { get; set; }
    }
}
