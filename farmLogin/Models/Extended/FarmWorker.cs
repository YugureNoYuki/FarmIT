using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace farmLogin.Models
{
    [MetadataType(typeof(FarmWorkerMetaData))]
    public partial class FarmWorker
    {
        public string JavaScriptToRun { get; set; }
    }
    public class FarmWorkerMetaData
    {
        [Key]
        public int FarmWorkerNum { get; set; }

        //if above "farmworkerNum" does not work, try using below snippet instead ("FarmWorkerId")
        //[Key]
        //public int FarmWorkerId { get; set; }

        [Required(ErrorMessage = "Name cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(maximumLength: 15, ErrorMessage = "Max 15 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "First Name must be alphabetic")]
        public string FarmWorkerFName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 15, ErrorMessage = "Max 15 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Last Name must be alphabetic")]
        public string FarmWorkerLName { get; set; }

        [Required(ErrorMessage = "Contact Number cannot be blank")]
        [Display(Name = "Contact Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Must be 10 characters")]
        public string FarmWorkerContactNum { get; set; }

        [Display(Name = "Farm Worker Image")]
        [StringLength(maximumLength: 255, ErrorMessage = "Max 255 characters reached")]
        public string FarmWorkerImg { get; set; }

        [Display(Name = "Address")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        //[DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Surburb")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Suburb must be made up of letters and numbers only")]
        public string Surburb { get; set; }

        [Display(Name = "City")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "City must be alphabetic")]
        public string City { get; set; }

        [Required(ErrorMessage = "Contract Start Date cannot be blank")]
        [Display(Name = "Contract Start Date")]
        //TODO: Validate future date selection
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime ContractStartDate { get; set; }

        [Required(ErrorMessage = "Contract End Date cannot be blank")]
        [Display(Name = "Contract End Date")]
        //TODO: Validate future date selection
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime ContractEndDate { get; set; }

        [Required(ErrorMessage = "ID Number cannot be blank")]
        [Display(Name = "ID Number")]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "Minimum of 11 characters and a maximum of 13.")]

        [RegularExpression("[0-9]+", ErrorMessage = "ID Number must be digits")]
        public string FarmWorkerIDNum { get; set; }

        public Nullable<bool> ClockedIn { get; set; }

    }
}