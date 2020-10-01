using MDL_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.LABELINSP.ViewModel
{
    public class PackingLabelViewModel
    {
        public PackingLabel PackingLabel { get; set; }
        public List<PackingLabelTest> PackingLabelTests { get; set; }
    }
}