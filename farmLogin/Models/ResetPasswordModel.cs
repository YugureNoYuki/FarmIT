using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "New Password")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "New Password required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}