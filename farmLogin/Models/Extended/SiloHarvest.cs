using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace farmLogin.Models
{
    [MetadataType(typeof(SiloHarvestMetaData))]
    public partial class SiloHarvest
    {

    }
    public class SiloHarvestMetaData
    {
        [Key]
        public int SiloHarvestID { get; set; }

        [Required(ErrorMessage = "Harvest Storage Date cannot be blank")]
        [Display(Name = "Harvest Storage Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime SiloHarvestStoreStartDate { get; set; }

        [Required(ErrorMessage = "Harvest Storage Expiry Date cannot be blank")]
        [Display(Name = "Harvest Storage Expiry Date")]
        //TODO: Validate future date selection
        [DataType(DataType.Date)]
        public System.DateTime SiloHarvestStoreEndDate { get; set; }

        [Required(ErrorMessage = "Harvest Tonnes Stored cannot be blank")]
        [Display(Name = "Harvest Tonnes Stored")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Invalid input format")]
        public decimal SiloHarvestTonnesStored { get; set; }
    }
}