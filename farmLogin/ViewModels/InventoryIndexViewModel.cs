using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using farmLogin.Models;
using System.Web.Mvc;
using PagedList; //adding paging

/// <summary>
/// View Model for Complex Filtering
/// </summary>

namespace farmLogin.ViewModels
{
    public class InventoryIndexViewModel
    {
        //public Inventory Inventory { get; set; }

        //public int InventoryID { get; set; }
        //public string InvDescr { get; set; }
        //public int InvQty { get; set; }
        //public System.DateTime InvDatePurchased { get; set; }
        //public string InvCode { get; set; }
        //public int Unit { get; set; }
        //public int InvTypeID { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InventoryTreatment> InventoryTreatments { get; set; }
        ////public virtual InventoryType InventoryType { get; set; }
        //public virtual Unit Unit1 { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderLine> OrderLines { get; set; }


        public IPagedList<Inventory> Inventories { get; set; } //IPageList
        //public IQueryable<Inventory> Inventories { get; set; }
        public string Search { get; set; }
        public IEnumerable<InventoryTypesWithCount> InvTypesWithCount { get; set; }
        public string InventoryType { get; set; }

        public IEnumerable<SelectListItem> InvTypeFilterItems
        {
            get
            {
                var allInvTypes = InvTypesWithCount.Select(tc => new SelectListItem
                {
                    Value = tc.InvTypeDescr,
                    Text = tc.InvDescrWithCount
                });

                return allInvTypes;
            }
        }

        public string JavaScriptToRun { get; set; }
    }

    //Holds InvType name and number of items within that InvType
    public class InventoryTypesWithCount
    {
        public int InventoryCount { get; set; }
        public string InvTypeDescr { get; set; }
        public string InvDescrWithCount {

            get
            {
                return InvTypeDescr + "(" + InventoryCount.ToString() + ")";
            }
        }
    }

}