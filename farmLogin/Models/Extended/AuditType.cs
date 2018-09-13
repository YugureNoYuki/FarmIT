using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(AuditTypeMetaData))]
    public partial class AuditType
    {

    }
    public class AuditTypeMetaData
    {
        [Key]
        public int AuditTypeID { get; set; }

        [Required(ErrorMessage = "Audit Type cannot be blank")]
        [Display(Name = "Audit Type")]
        [StringLength(maximumLength: 50, ErrorMessage = "Max 50 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Audit Type must be alphabetic")]
        public string AuditTypeDescr { get; set; }
    }
}