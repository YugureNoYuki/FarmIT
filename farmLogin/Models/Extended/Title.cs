using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace farmLogin.Models
{
    [MetadataType(typeof(TitleMetaData))]
    public partial class Title
    {

    }
    public class TitleMetaData
    {
        [Key]
        public int TitleID { get; set; }



        [Required(ErrorMessage = "Title cannot be blank")]
        [Display(Name = "Title")]
        [StringLength(maximumLength: 8, ErrorMessage = "Max 8 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Title must be alphabetic")]
        public string TitleDescr { get; set; }
    }
}