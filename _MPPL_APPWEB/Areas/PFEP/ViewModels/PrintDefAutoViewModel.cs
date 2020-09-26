using MDL_BASE.Models.Base;

using MDL_BASE.Models.MasterData;
using MDL_PFEP.ComponentLineFeed.Models;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.DEF;
//using MDL_PFEP.Models.PFEP;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class PrintDefAutoViewModel
    {
        public int Shift { get; set; }
        public DateTime Date { get; set; }
        public List<Routine> Routines { get; set; }
        public List<PrintDefViewModel> PrintDefsViewModels { get; set; }
    }
}

