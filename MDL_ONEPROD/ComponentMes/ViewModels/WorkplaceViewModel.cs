using MDL_ONEPROD.ComponentMes.Enums;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class WorkplaceViewModel
    {
        //public DateTime DateFrom { get; set; }
        //public DateTime DateTo { get; set; }
        //public int SelectedWorkplaceId { get; set; }
        //public Workplace Workplace { get; set; }
        //public List<Workplace> Workplaces { get; set; }
        //public List<ReasonType> ReasonTypes { get; set; }
        //public string IP { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int? ResourceGroupId { get; set; }
        public int? SelectedWorkorderId { get; set; }
        public string ComputerHostName { get; set; }
        public string PrinterIPv4 { get; set; }
        public string LoggedUserName { get; set; }
        public string LabelANC { get; set; }
        public string LabelName { get; set; }
        public bool PrintLabel { get; set; }
        public SerialNumberType SerialNumberType { get; set; }
        public PrinterType PrinterType { get; set; }
        public WorkplaceTypeEnum Type { get; set; }
        public int LabelLayoutNo { get; set; }
        public bool IsTraceability { get; set; }
        public bool IsReportOnline { get; set; }
        public bool Deleted { get; set; }
    }
}