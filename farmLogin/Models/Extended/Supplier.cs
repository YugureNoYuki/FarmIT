using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using System.Text.RegularExpressions;

namespace farmLogin.Models
{
    [MetadataType(typeof(SupplierMetaData))]
    public partial class Supplier
    {
        public string JavaScriptToRun { get; set; }
    }
    public class SupplierMetaData
    {

        [Key]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Supplier Name cannot be blank")]
        [Display(Name = "Supplier Name")]
        [StringLength(maximumLength: 50, ErrorMessage = "Max 50 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Name must be alphabetic")]
        public string SupplierName { get; set; }

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

        [Required(ErrorMessage = "Email cannot be blank")]
        [Display(Name = "Email")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        // Need to Test if this works
        [RegularExpression("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Please Enter a Valid Email Address")]
        
        public string SupplierEmailAddress { get; set; }

        [Required(ErrorMessage = "Phone number cannot be blank")]
        [Display(Name = "Phone number")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Phone number must be numeric")]
        public string SupplierPhoneNum { get; set; }

        [Required(ErrorMessage = "Cellphone number cannot be blank")]
        [Display(Name = "Cellphone number")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Cellphone number must be numeric")]
        public string SupplierCellNum { get; set; }
    }
}