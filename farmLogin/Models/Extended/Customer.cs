using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer
    {
        public string JavaScriptToRun { get; set; }
    }
    public class CustomerMetaData
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "First Name cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(maximumLength: 15, ErrorMessage = "Max 15 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "First Name must be alphabetic")]
        public string CustomerFName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 15, ErrorMessage = "Max 15 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Last Name must be alphabetic")]
        public string CustomerLName { get; set; }

        [Required(ErrorMessage = "Contact Number cannot be blank")]
        [Display(Name = "Contact Number")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Contact number must be numeric")]
        public string CustomerContactNum { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [Display(Name = "Email")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        // Need to Test if this works
        [RegularExpression("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please Enter a Valid Email Address")]
        public string ContactPersonalEmailAddress { get; set; }

        [Required(ErrorMessage = "Company Name cannot be blank")]
        [Display(Name = "Company Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Company Name must be alphabetic")]
        public string CompanyName { get; set; }

        [Display(Name = "Address")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        public string Address { get; set; }

        [Display(Name = "Surburb")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Suburb must be made up of letters and numbers only")]
        public string Surburb { get; set; }

        [Display(Name = "City")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "City must be alphabetic")]
        public string City { get; set; }

        [Required(ErrorMessage = "Telephone number cannot be blank")]
        [Display(Name = "Telephone number")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Telephone number must be numeric")]
        public string CompanyTelNum { get; set; }
    }
}