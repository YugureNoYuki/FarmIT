using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(OrderStatusMetaData))]
    public partial class OrderStatu
    {

    }
    public class OrderStatusMetaData
    {

        [Key]
        public int OrderStatusID { get; set; }


        [Required(ErrorMessage = "Status cannot be blank")]
        [Display(Name = "Status")]
        [StringLength(maximumLength: 20, ErrorMessage = "Max 20 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Status must be alphabetic")]
        public string OrderStatusDescr { get; set; }

    }
}