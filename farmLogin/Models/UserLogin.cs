using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    public class UserLogin
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string UserEmailAddress { get; set; }

        [Display(Name ="Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}