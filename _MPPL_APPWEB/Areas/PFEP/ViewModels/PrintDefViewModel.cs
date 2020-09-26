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
    public class PrintDefViewModel
    {
        public int PrintNumber { get; set; }
        public DateTime PrintDate { get; set; }
        public Routine RoutineObj { get; set; }
        public string Routine { get; set; }
        public string DateRange { get; set; }
        public string Defs { get; set; }
        public string Lines { get; set; }
        public int LineId { get; set; }

        //public PrintData PrintData { get; set; }
        public List<PrintDataMatrix> PrintDataMatrixes { get; set; }
    }
}

