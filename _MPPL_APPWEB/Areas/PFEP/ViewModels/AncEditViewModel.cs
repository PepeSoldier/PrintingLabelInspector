
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class AncEditViewModel
    {
        public Item AncObject { get; set; }
        public IEnumerable<SelectListItem> AncTypes { get; set; }
    }
}