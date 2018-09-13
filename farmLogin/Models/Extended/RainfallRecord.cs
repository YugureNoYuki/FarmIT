using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(RainfallRecordMetaData))]
    public partial class RainfallRecord
    {
        public string JavaScriptToRun { get; set; }
    }
    public class RainfallRecordMetaData
    {

        [Key]
        public int RainFallRecID { get; set; }

        [Required(ErrorMessage = "Record Date cannot be blank")]
        [Display(Name = "Record Date")]
        //TODO: Validate future date selection (should be past date?)
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Date cannot be a future date")]
        public System.DateTime RecordDate { get; set; }

        [Required(ErrorMessage = "Record measurement cannot be blank")]
        [Display(Name = "Measurement")]
        [RegularExpression("[0-9]+", ErrorMessage = "Measurement must be numeric")]
        [Range(minimum: 1, maximum: 999999, ErrorMessage = "Measurement cannot be negative")]
        public int RecordMeasurement { get; set; }
    }
}