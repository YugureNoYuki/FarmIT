using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using farmLogin.Models;
using PagedList;

namespace farmLogin.ViewModels
{
    public class CropCycleIndexViewModel
    {
        // public IEnumerable<CropCycle> CropCycles { get; set; }
        public IPagedList<CropCycle> CropCycles { get; set; }
        public string Search { get; set; }

        public string JavaScriptToRun { get; set; }
    }
}