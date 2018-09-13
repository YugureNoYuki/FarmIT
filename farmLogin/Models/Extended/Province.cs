using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(ProvinceMetaData))]
    public partial class Province
    {

    }
    public class ProvinceMetaData
    {

        [Key]
        public int ProvinceID { get; set; }


        [Required(ErrorMessage = "Province Description cannot be blank")]
        [Display(Name = "Province")]
        [StringLength(maximumLength: 36, ErrorMessage = "Max 36 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Province Description  must be alphabetic")]
        public string ProvinceDescr { get; set; }
    }
}