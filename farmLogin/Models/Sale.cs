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
    
    public partial class Sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sale()
        {
            this.SiloHarvestSales = new HashSet<SiloHarvestSale>();
        }
    
        public int SaleID { get; set; }
        public System.DateTime SaleDate { get; set; }
        public decimal SaleQty { get; set; }
        public int UnitID { get; set; }
        public decimal SaleAmnt { get; set; }
        public int CustomerID { get; set; }
        public string PurchaseAgreement { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SiloHarvestSale> SiloHarvestSales { get; set; }
    }
}