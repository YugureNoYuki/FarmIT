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
    
    public partial class FieldTreatment
    {
        public int FieldID { get; set; }
        public int TreatmentID { get; set; }
        public System.DateTime TreatmentDate { get; set; }
    
        public virtual Field Field { get; set; }
        public virtual Treatment Treatment { get; set; }
    }
}
