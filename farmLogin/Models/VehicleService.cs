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
    
    public partial class VehicleService
    {
        public int VehicleServiceID { get; set; }
        public System.DateTime VehicleService_Date { get; set; }
        public decimal VehicleService_Cost { get; set; }
        public int VehicleServiceRecord { get; set; }
        public int UnitID { get; set; }
        public int VehicleID { get; set; }
    
        public virtual Unit Unit { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
