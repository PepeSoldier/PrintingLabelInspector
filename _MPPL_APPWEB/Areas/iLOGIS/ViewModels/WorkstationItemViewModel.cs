using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.iLOGIS.ViewModels
{
    public class WorkstationItemViewModel
    {
        public int Id { get; set; }
        public int WorkstationId { get; set; }
        public string WorkstationName { get; set; }
        public string LineName { get; set; }
        //public string WorkstationDescr { get; set; }
        public string PutTo { get; set; }

        public int ItemWMSId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public int MaxPackages { get; set; }
        public int SafetyStock { get; set; }
        public int MaxBomQty { get; set; }
        public bool CheckOnly { get; set; }
    }
}