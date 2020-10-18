using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDLX_CORE.ViewModel
{
    public class BOMViewModel
    {
        public int Id { get; set; }

        public int? ChildId { get; set; }
        public string ChildCode { get; set; }
        public string ChildName { get; set; }

        public int? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }

        public int LV { get; set; }
        public decimal QtyUsed { get; set; } //pieces - ilośc sztuk

        public string BC { get; set; }
        public string DEF { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string IDCO { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}