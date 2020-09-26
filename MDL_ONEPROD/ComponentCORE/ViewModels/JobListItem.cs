using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentCORE.ViewModels
{
    public class JobListItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal Qty { get; set; }
        public int PhotoPosition { get; set; }
        public int ParentItemId { get; set; }
        public string Prefix { get; set; }
    }
}