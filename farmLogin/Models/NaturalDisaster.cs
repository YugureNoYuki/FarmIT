//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace farmLogin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NaturalDisaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NaturalDisaster()
        {
            this.FieldNaturalDisasters = new HashSet<FieldNaturalDisaster>();
        }
    
        public int NatDisasterID { get; set; }
        public string NatDisasterDescr { get; set; }
        public string NatDisasterPrecautions { get; set; }
        public string NatDisasterPotentialEffects { get; set; }
        public string NatDisasterSigns { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FieldNaturalDisaster> FieldNaturalDisasters { get; set; }
    }
}
