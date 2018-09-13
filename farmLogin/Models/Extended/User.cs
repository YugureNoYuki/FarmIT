using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
        public string JavaScriptToRun { get; set; }
    }
    public class UserMetaData
    {
        [Key]
        public int UserID { get; set; }

        //[Required(ErrorMessage = "Username cannot be blank")]
        //[Display(Name = "Username")]
        //[StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        //[RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Description must be made up of letters and numbers only")]
        //public string UserName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [Display(Name = "Email")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [DataType(DataType.EmailAddress)]
        public string UserEmailAddress { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        //[RegularExpression(("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})"))]
        public string UserPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("UserPassword",ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Contact Number")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Contact number must be numeric")]
        public string UserContactNum { get; set; }

        [Required(ErrorMessage = "First Name cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "First Name must be alphabetic")]
        public string UserFName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Last Name must be alphabetic")]
        public string UserLName { get; set; }

        [Required(ErrorMessage = "ID Number cannot be blank")]
        [Display(Name = "ID Number")]
        [StringLength(maximumLength: 13, ErrorMessage = "Max 13 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "ID Number must be digits")]
        public string UserIDNum { get; set; }

        public Nullable<bool> IsEmailVerified { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}