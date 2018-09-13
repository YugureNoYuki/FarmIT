using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(AttendenceSheetMetaData))]
    public partial class AttendenceSheet
    {
        public string JavaScriptToRun { get; set; }
    }
    public class AttendenceSheetMetaData
    {
        [Key]
        public int AttendenceSheetID { get; set; }

        [Required(ErrorMessage = "Clock In Time cannot be blank")]
        [Display(Name = "Clock In Time")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.TimeSpan ClockInTime { get; set; }

        [Display(Name = "Clock Out Time")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Date must be before or on Today")]
        public System.TimeSpan ClockOutTime { get; set; }

    }
}