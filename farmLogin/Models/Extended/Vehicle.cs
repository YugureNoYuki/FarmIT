using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;

namespace farmLogin.Models
{
    [MetadataType(typeof(VehicleMetaData))]
    public partial class Vehicle
    {
        public string JavaScriptToRun { get; set; }
    }
    public class VehicleMetaData
    {

        [Key]
        public int VehicleID { get; set; }

        [Required(ErrorMessage = "Vehicle Name cannot be blank")]
        [Display(Name = "Vehicle Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Vehicle Name must be made up of letters and numbers only")]
        public string VehName { get; set; }

        [Display(Name = "Year")]
        [StringLength(maximumLength: 10, ErrorMessage = "Max 10 characters reached")]
        [RegularExpression("[0-9]+", ErrorMessage = "Year must be numeric")]
        //[Range(minimum: 4, maximum: 4, ErrorMessage = "Year has to be in the format 'YYYY'")]
        public string VehYear { get; set; }

        [Required(ErrorMessage = "Model cannot be blank")]
        [Display(Name = "Model")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Model must be made up of letters and numbers only")]
        public string VehModel { get; set; }

        
        [Display(Name = "Engine Number")]
        [StringLength(maximumLength: 30, ErrorMessage = "Max 30 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Engine Number must be made up of letters and numbers only")]
        public string VehEngineNum { get; set; }

        [Required(ErrorMessage = "VIN Number cannot be blank")]
        [Display(Name = "VIN Number")]
        [StringLength(maximumLength: 17, ErrorMessage = "Max 17 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "VIN Number must be made up of letters and numbers only")]
        public string VehVinNum { get; set; }

        [Required(ErrorMessage = "Registration Number cannot be blank")]
        [Display(Name = "Registration Number")]
        [StringLength(maximumLength: 12, ErrorMessage = "Max 12 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Registration Number must be made up of letters and numbers only")]
        public string VehRegNum { get; set; }

        [Required(ErrorMessage = "License Number cannot be blank")]
        [Display(Name = "License Number")]
        [StringLength(maximumLength: 15, ErrorMessage = "Max 15 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "License Number must be made up of letters and numbers only")]
        public string VehLicenseNum { get; set; }

        [Required(ErrorMessage = "License Expiry Date cannot be blank")]
        [Display(Name = "License Expiry Date")]
        //TODO: Validate past date selection
        [DataType(DataType.Date)]
        [MyPast(ErrorMessage = "Date must be after Today")]

        public Nullable<System.DateTime> VehExpDate { get; set; }

        [Required(ErrorMessage = "Mileage cannot be blank")]
        [Display(Name = "Mileage")]
        [RegularExpression("[0-9]+", ErrorMessage = "Mileage must be numeric")]
        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Mileage cannot be negative or zero")]
        public int VehCurrMileage { get; set; }

        [Required(ErrorMessage = "Service Interval cannot be blank")]
        [Display(Name = "Service Interval")]
        [RegularExpression("[0-9]+", ErrorMessage = "Interval must be numeric")]
        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Interval cannot be negative or zero")]
        public int VehServiceInterval { get; set; }

        [Display(Name = "Next Service")]
        //TODO: Validate future date selection
        //[DataType(DataType.Date)]
        public int VehNextService { get; set; } //calculate
    }
}