using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(SiloMetaData))]
    public partial class Silo
    {
        public string JavaScriptToRun { get; set; }
    }
    public class SiloMetaData
    {

        [Key]
        public int SiloID { get; set; }

        [Required(ErrorMessage = "Silo cannot be blank")]
        [Display(Name = "Silo")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Silo description must be alphabetic")]
        public string SiloDescr { get; set; }

        [Required(ErrorMessage = "Silo capacity cannot be blank")]
        [Display(Name = "Silo Capacity")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public Nullable<decimal> SiloCapacity { get; set; }

        [Required(ErrorMessage = "Silo Rental Fee cannot be blank")]
        [Display(Name = "Silo Rental Fee (P/A)")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public Nullable<decimal> SiloRentalFeePA { get; set; }

        //[Required(ErrorMessage = "Silo Status cannot be blank")]
        [Display(Name = "Silo Status")]
        [StringLength(maximumLength: 20, ErrorMessage = "Max 20 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Silo Status description must be alphabetic")]
        public string SiloStatus { get; set; }

    }
}