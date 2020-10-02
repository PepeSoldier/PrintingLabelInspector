using MDL_LABELINSP.Entities;
using System.Collections.Generic;

namespace MDL_LABELINSP.ViewModel
{
    public class PackingLabelViewModel
    {
        public PackingLabel PackingLabel { get; set; }
        public List<PackingLabelTest> PackingLabelTests { get; set; }
    }
}