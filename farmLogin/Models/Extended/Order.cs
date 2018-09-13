using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(OrderMetaData))]
    public partial class Order
    {

    }
    public class OrderMetaData
    {

        [Key]
        public int OrderNum { get; set; }

        //if above snippet does not work, try below snippet ("OrderId")
        //[Key]
        //public int OrderId { get; set; }

        [Required(ErrorMessage = "Order date cannot be blank")]
        [Display(Name = "Order Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Item Price cannot be blank")]
        [Display(Name = "Item Price")]
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal OrderItemPrice { get; set; }

        [Required(ErrorMessage = "Item cannot be blank")]
        [Display(Name = "Item Name")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Item Name must be made up of letters and numbers only")]
        public string OrderItem { get; set; }

        [Required(ErrorMessage = "Quantity cannot be blank")]
        [Display(Name = "Order Quantity")]
        [RegularExpression("[0-9]+", ErrorMessage = "Quantity must be numeric")]
        [Range(minimum: 1, maximum: 999999, ErrorMessage = "Quantity cannot be negative")]
        public int OrderQty { get; set; }

    }
}