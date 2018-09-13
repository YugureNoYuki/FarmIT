using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(LogMetaData))]
    public partial class Log
    {

    }
    public class LogMetaData
    {

        [Key]
        public int LogID { get; set; }


        [Required(ErrorMessage = "Log In Time cannot be blank")]
        [Display(Name = "Log In Time")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.TimeSpan LogInTimeStamp { get; set; }


        [Required(ErrorMessage = "Log Out Time cannot be blank")]
        [Display(Name = "Log Out Time")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.TimeSpan LogOutTimeStamp { get; set; }
    }
}