using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentConfig.ViewModels
{
    public class ItemWMSGridViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public int ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public string DEF { get; set; }
        public string BC { get; set; }
        public int H { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string PREFIX { get; set; }
        public int PickerNo { get; set; }
        public int TrainNo { get; set; }
        public decimal Weight { get; set; }
        public ItemTypeEnum Type { get; set; }
        public string StartDate { get; set; }
        public DateTime StartDateTmp { get; set; }
        public ClassificationABCEnum ABC { get; set; }
        public ClassificationXYZEnum XYZ { get; set; }
    }
}
