using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS
{
    public class WorkorderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public string ItemGroupColor { get; set; }
        public string ItemColor1 { get; set; }
        public string ItemColor2 { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }

        public int Qty_Total { get; set; }
        public int Qty_Produced { get; set; }
        public int Qty_Used { get; set; }
        public int Qty_Remain { get { return Qty_Total - Qty_Produced; } }
        public int BatchNumber { get; set; }
        public int LV { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int ProcessingTime { get; set; }

        public int ClientOrderId { get; set; }
        public string ClientOrderNo { get; set; }

        public int? ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int? ResourceGroupId { get; set; }
        public bool ResourceGroupShowBatches { get; set; }

        public int? ToolId { get; set; }
        public string ToolName { get; set; }
        public int Param1 { get; set; }

        public TaskScheduleStatus Status { get; set; }

        //public double Width { get; set; }
        //public string WidthStr { get { return Width.ToString("0.0").Replace(',', '.'); } }
        //public string SpecialCssClass { get; set; }
        //public double MarginLeft { get; set; }
        //public string MarginLeftStr { get { return MarginLeft.ToString("0.0").Replace(',', '.'); } }
        //public double MarginRight { get; set; }

    }
}