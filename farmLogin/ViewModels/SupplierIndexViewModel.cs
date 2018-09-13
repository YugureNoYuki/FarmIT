using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using farmLogin.Models;

namespace farmLogin.ViewModels
{
    public class SupplierIndexViewModel
    {
        public IPagedList<Supplier> Suppliers { get; set; } //IPageList
        public string Search { get; set; }
        public string SupplierName { get; set; }
        public string JavaScriptToRun { get; set; }
    }
}