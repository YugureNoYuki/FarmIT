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
    
    public partial class Audit
    {
        public int AuditID { get; set; }
        public string AuditRefTable { get; set; }
        public System.DateTime TrxDate { get; set; }
        public System.DateTime TrxTime { get; set; }
        public string TrxNewRecord { get; set; }
        public string TrxOldRecord { get; set; }
        public string AuditRefAtrribute { get; set; }
        public int UserID { get; set; }
        public int AuditTypeID { get; set; }
    
        public virtual AuditType AuditType { get; set; }
        public virtual User User { get; set; }
    }
}
