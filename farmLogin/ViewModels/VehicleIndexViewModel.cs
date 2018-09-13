using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using farmLogin.Models;
using System.Web.Mvc;

namespace farmLogin.ViewModels
{
    public class VehicleIndexViewModel
    {

        public IPagedList<Vehicle> Vehicles { get; set; } //IPageList
        //public IQueryable<Vehicle> Vehicles { get; set; }
        public string Search { get; set; }
        public IEnumerable<VehicleTypesWithCount> VehTypesWithCount { get; set; }
        public List<VehicleService> VehicleServices { get; set; }
        public Vehicle Vehicle { get; set; }
        public string VehicleType { get; set; }
        public string JavaScriptToRun { get; set; }

        public IEnumerable<SelectListItem> VehTypeFilterItems
        {
            get
            {
                var allInvTypes = VehTypesWithCount.Select(tc => new SelectListItem
                {
                    Value = tc.VehTypeDescr,
                    Text = tc.VehDescrWithCount
                });

                return allInvTypes;
            }
        }
    }
    //Holds InvType name and number of items within that VehType
    public class VehicleTypesWithCount
    {
        public int VehicleCount { get; set; }
        public string VehTypeDescr { get; set; }
        public string VehDescrWithCount
        {

            get
            {
                return VehTypeDescr + "(" + VehicleCount.ToString() + ")";
            }
        }
    }


}