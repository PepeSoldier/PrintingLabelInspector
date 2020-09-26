using MDL_BASE.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.AP.ViewModel.Def
{
    public interface IDefManageViewModel
    {
        List<IDefModel> ObjectList { get; set; }
        IDefModel NewObject { get; set; }
        string Titled { get; set; }  
    }

    public interface IDefManageComboViewModel
    {
        List<IDefComboModel> ObjectList { get; set; }
        IDefComboModel NewObject { get; set; }
        IEnumerable<SelectListItem> ComboObjectsList { get; set; }

        string Titled { get; set; }
        int SelectedComboId { get; set; }
    }
}