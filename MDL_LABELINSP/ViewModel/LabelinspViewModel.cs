using MDL_LABELINSP.Entities;
using System.Collections.Generic;

namespace MDL_LABELINSP.ViewModel
{
    public class LabelinspViewModel
    {
        public WorkorderLabel WorkorderLabel { get; set; }
        public List<WorkorderLabelInspection> WorkorderLabelInspections { get; set; }
    }
}