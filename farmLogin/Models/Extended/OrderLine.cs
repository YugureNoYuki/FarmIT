using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(OrderLineMetaData))]
    public partial class OrderLine
    {

    }
    public class OrderLineMetaData
    {
        [Key, Column(Order = 0)]
        public int OrderNum { get; set; }
        [Key, Column(Order = 1)]
        public int InventoryID { get; set; }

        [Required(ErrorMessage = "Total Amount cannot be blank")]
        [Display(Name = "Total Amount")]
       // [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal OrderLineTotalAmount { get; set; }

        [Required(ErrorMessage = "Quantity cannot be blank")]
        [Display(Name = "Quantity")]
        [RegularExpression("[0-9]+", ErrorMessage = "Quantity must be numeric")]
        [Range(minimum: 1, maximum: 999999, ErrorMessage = "Quantity cannot be negative or zero")]
        public int OrderLineTotalQuantity { get; set; }
    }
}