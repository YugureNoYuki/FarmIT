using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(NaturalDisasterMetaData))]
    public partial class NaturalDisaster
    {

    }
    public class NaturalDisasterMetaData
    {

        [Key]
        public int NatDisasterID { get; set; }


        [Required(ErrorMessage = "Natural Disaster cannot be blank")]
        [Display(Name = "Natural Disaster")]
        [StringLength(maximumLength: 35, ErrorMessage = "Max 35 characters reached")]
        [RegularExpression(@"^[a-zA-Z'-'\s]*$", ErrorMessage = "Natural Disaster description must be alphabetic")]
        public string NatDisasterDescr { get; set; }

        [Display(Name = "Natural Disaster Precaution")]
        [StringLength(maximumLength: 100, ErrorMessage = "Max 100 characters reached")]
        public string NatDisasterPrecautions { get; set; }

        [Display(Name = "Natural Disaster Potential Effect")]
        [StringLength(maximumLength: 100, ErrorMessage = "Max 100 characters reached")]
        public string NatDisasterPotentialEffects { get; set; }

        [Display(Name = "Natural Disaster Sign")]
        [StringLength(maximumLength: 100, ErrorMessage = "Max 100 characters reached")]
        public string NatDisasterSigns { get; set; }


    }
}