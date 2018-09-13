using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(GenderMetaData))]
    public partial class Gender
    {

    }
    public class GenderMetaData
    {

        [Key]
        public int GenderID { get; set; }


        [Required(ErrorMessage = "Gender cannot be blank")]
        [Display(Name = "Gender")]
        [StringLength(maximumLength: 8, ErrorMessage = "Max 8 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Gender description must be alphabetic")]
        public string GenderDescr { get; set; }

    }
}