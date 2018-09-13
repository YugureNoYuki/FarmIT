using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmLogin.Models
{
    [MetadataType(typeof(FieldNaturalDisasterMetaData))]
    public partial class FieldNaturalDisaster
    {

    }
    public class FieldNaturalDisasterMetaData
    {
        [Key, Column(Order = 0)]
        public int FieldID { get; set; }
        [Key, Column(Order = 1)]
        public int NatDisasterID { get; set; }


        [Required(ErrorMessage = "Date cannot be blank")]
        [Display(Name = "Date")]
        //TODO: Validate future date selection (past?)
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
    }
}