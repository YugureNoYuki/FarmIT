using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(CountryMetaData))]
    public partial class Country
    {

    }
    public class CountryMetaData
    {
        [Key]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Country cannot be blank")]
        [Display(Name = "Country")]
        [StringLength(maximumLength: 25, ErrorMessage = "Max 25 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Country must be alphabetic")]
        public string CountryDescr { get; set; }
    }
}