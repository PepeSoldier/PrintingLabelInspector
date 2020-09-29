using MDL_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_CORE.ComponentCore.ViewModel
{
    public class PackingLabelViewModel
    {
        public PackingLabel PackingLabel { get; set; }
        public List<PackingLabelTest> PackingLabelTests { get; set; }
    }
}