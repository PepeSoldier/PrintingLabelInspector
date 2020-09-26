using MDL_AP.Interfaces;
using MDL_BASE.Interfaces;
using XLIB_COMMON.Interface;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.AP.ViewModel.Def
{
    public class DefComboViewModel : IDefManageComboViewModel
    {
        public List<IDefComboModel> ObjectList { get; set; }
        public IDefComboModel NewObject { get; set; }
        public IEnumerable<SelectListItem> ComboObjectsList { get; set; }

        public string Titled { get; set; }
        public int SelectedComboId { get; set; }
    }
}