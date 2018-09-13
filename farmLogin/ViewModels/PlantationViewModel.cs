using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using farmLogin.Models;
using System.Web.Mvc;

namespace farmLogin.ViewModels
{
    public class PlantationViewModel
    {
        public IPagedList<Plantation> Plantations { get; set; }
        public string Search { get; set; }
        public string CropType { get; set; }
        public IEnumerable<CropTypesWithCount> cropTypesWithCount { get; set; }
        public IEnumerable<SelectListItem> CropTypeFilterItems
        {
            get
            {
                var allCropTypes = cropTypesWithCount.Select(tc => new SelectListItem
                {
                    Value = tc.CropTypeDescr,
                    Text = tc.CropTypeDescrWithCount
                });

                return allCropTypes;
            }
        }
    }

    //Holds CropType name and number of plantations within that CropType
    public class CropTypesWithCount
    {
        public int PlantationCount { get; set; }
        public string CropTypeDescr { get; set; }
        public string CropTypeDescrWithCount
        {

            get
            {
                return CropTypeDescr + "(" + PlantationCount.ToString() + ")";
            }
        }
    }
}