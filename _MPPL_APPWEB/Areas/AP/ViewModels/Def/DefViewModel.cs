using MDL_AP.Interfaces;
using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.AP.ViewModel.Def
{
    public class DefViewModel : IDefManageViewModel
    {
        public virtual List<IDefModel> ObjectList { get; set; }
        public virtual IDefModel NewObject { get; set; }
        public string Titled { get; set; }
    }
}