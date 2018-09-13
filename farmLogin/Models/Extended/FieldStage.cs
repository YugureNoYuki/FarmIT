using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(FieldStageMetaData))]
    public partial class FieldStage
    {

    }
    public class FieldStageMetaData
    {
        [Key]
        public int FieldStageID { get; set; }


        [Required(ErrorMessage = "Field Stage cannot be blank")]
        [Display(Name = "Field Stage")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Field Stage description must be alphabetic")]
        public string FieldStageDescr { get; set; }
    }
}