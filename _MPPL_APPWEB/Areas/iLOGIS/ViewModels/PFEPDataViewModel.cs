using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.iLOGIS.ViewModels
{
    public class PFEPDataViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public int WorkstationId { get; set; }
        public string WorkstationName { get; set; }
        public string WorkstationSortOrder { get; set; }
        public int WorkstationLineId { get; set; }
        public string WorkstationLineName { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageCode { get; set; }
        public int PackageW { get; set; }
        public int PackageH { get; set; }
        public int PackageD { get; set; }
    }
}