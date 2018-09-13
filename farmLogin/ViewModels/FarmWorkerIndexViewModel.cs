using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using farmLogin.Models;
using System.Web.Mvc;

namespace farmLogin.ViewModels
{
    public class FarmWorkerIndexViewModel
    {
        public IPagedList<FarmWorker> FarmWorkers { get; set; } //IPageList
        //public IQueryable<FarmWorker> FarmWorkers { get; set; }
        public string Search { get; set; }
        public IEnumerable<FarmWorkerTypesWithCount> WorkerTypesWithCount { get; set; }
        public string FarmWorkerType { get; set; }
        public string JavaScriptToRun { get; set; }

        public IEnumerable<SelectListItem> FarmWorkerTypeFilterItems
        {
            get
            {
                var allInvTypes = WorkerTypesWithCount.Select(tc => new SelectListItem
                {
                    Value = tc.FarmWorkerTypeDescr,
                    Text = tc.WrkrDescrWithCount
                });

                return allInvTypes;
            }
        }
    }

    //Holds InvType name and number of items within that InvType
    public class FarmWorkerTypesWithCount
    {
        public int FarmWorkerCount { get; set; }
        public string FarmWorkerTypeDescr { get; set; }
        public string WrkrDescrWithCount
        {

            get
            {
                return FarmWorkerTypeDescr + "(" + FarmWorkerCount.ToString() + ")";
            }
        }
    }
}