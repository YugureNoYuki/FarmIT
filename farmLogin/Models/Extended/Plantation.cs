using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(PlantationMetaData))]
    public partial class Plantation
    {
        public string JavaScriptToRun { get; set; }

    }
    public class PlantationMetaData
    {
        [Key]
        public int PlantationID { get; set; }

        [Required(ErrorMessage = "Date Planted cannot be blank")]
        [Display(Name = "Date Planted")]
        //TODO: Validate future date selection
        //[DataType(DataType.Date)]
        //[MyPast(ErrorMessage = "Date must be after Today")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DatePlanted { get; set; }

        [Display(Name = "Refuge Seed Amount")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public Nullable<decimal> RefugeSeedAmntUsed { get; set; }

        [Display(Name = "Refuge Area Hectares")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        
        public Nullable<decimal> RefugeAreaHectares { get; set; }

        [Required(ErrorMessage = "Expectied Yield Qty cannot be blank")]
        [Display(Name = "Expectied Yield Qty")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal ExpectedYieldQnty { get; set; }

        [Display(Name = "Date Harvested")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Date must be before or on Today")]
        public Nullable<System.DateTime> DateHarvested { get; set; }

        //[Required(ErrorMessage = "Plantatioin Status cannot be blank")]
        [Display(Name = "Plantation Status")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Plantation Status must be alphabetic")]
        public string PlantationStatus { get; set; }
    }
}