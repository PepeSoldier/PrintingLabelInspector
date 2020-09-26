using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class WorkplaceBufferViewModel
    {
        public WorkplaceBufferViewModel()
        {

        }
        public int Id { get; set; }
        public int ParentItemId { get; set; }
        public int ChildItemId { get; set; }
        public string ChildCode { get; set; }
        public string SerialNumber { get; set; }
        public string ChildName { get; set; }
        public string Barcode { get; set; }
        public decimal QtyInBom { get; set; } 
        public decimal QtyRequested { get; set; } 
        public decimal QtyAvailable { get; set; } 
        public int ProcessId { get; set; }
        public int WorkorderId { get; set; }
        public string WorkorderNumber { get; set; }
        public string TimeLoaded { get; set; }
    }
}