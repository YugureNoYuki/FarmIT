using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(UserAccessLevelMetaData))]
    public partial class UserAccessLevel
    {
        public string JavaScriptToRun { get; set; }
    }
    public class UserAccessLevelMetaData
    {
        [Key]
        public int UserAccessLevelID { get; set; }

        [Required(ErrorMessage = "User Access Role cannot be blank")]
        [Display(Name = "User Access Role")]
        [StringLength(maximumLength: 25, ErrorMessage = "Max 25 characters reached")]
        public string UserAccessLevelDescr { get; set; }

        [Required(ErrorMessage = "Notes cannot be blank")]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength: 255, ErrorMessage = "Max 50 characters reached")]
        public string Notes { get; set; }
    }
}