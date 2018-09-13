using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(FieldMetaData))]
    public partial class Field
    {
        public string JavaScriptToRun { get; set; }

    }
    public partial class FieldMetaData
    {
        [Key]
        public int FieldID { get; set; }

        [Required(ErrorMessage = "Field Name cannot be blank")]
        [Display(Name = "Field Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Field Name must be made up of letters and numbers only")]
        public string FieldName { get; set; }

        [Required(ErrorMessage ="Hectares cannot be blank")]
        [Display(Name = "Hectares")]
        //[DataType(DataType.Currency)]
        //[RegularExpression(@"^[0-9]*(?:\,[0-9]*)?$", ErrorMessage = "Invalid input format")]
        //[RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Please enter number with a '.'")]
        //[Range(1, 100, ErrorMessage = "Hectares must be between 1 and 100")]
        //[RegularExpression(@"^\d * (\.|,| (\.\d{1, 2})|(,\d{1,2}))?$", ErrorMessage = "my message")]
        //[RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal FieldHectares { get; set; }

    }
}