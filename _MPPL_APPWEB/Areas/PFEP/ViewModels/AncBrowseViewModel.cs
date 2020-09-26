using MDLX_MASTERDATA.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class AncBrowseViewModel
    {
        public IEnumerable<Item> Ancs { get; set; }

        //moze sie przydac do dropdownlisty fintrujacej wyniki
        public IEnumerable<SelectListItem> AncTypes { get; set; }
    }
}