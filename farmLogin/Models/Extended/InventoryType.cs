using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(InventoryTypeMetaData))]
    public partial class InventoryType
    {
        //public static implicit operator InventoryType(string v)
        //{
        //    throw new NotImplementedException();
        //}
        public static implicit operator InventoryType(string v)
        {
            throw new NotImplementedException();
        }
        public string JavaScriptToRun { get; set; }
    }
    public class InventoryTypeMetaData
    {

        [Key]
        public int InvTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Inventory type cannot be blank")]
        [Display(Name = "Inventory Type")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Inventory type description must be alphabetic")]
        public string InvTypeDescr { get; set; }
    }
}