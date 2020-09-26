using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentPFEP.Models
{
    public class PFEPData
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemClass { get; set; }
        public DateTime ItemCreatedDate { get; set; }
        public bool? ItemDeleted { get; set; }
        public int ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public int WorkstationId { get; set; }
        public string WorkstationName { get; set; }
        public int MaxBomQty { get; set; }
        public bool CheckOnly { get; set; }
        public int WorkstationSortOrder { get; set; }
        public int WorkstationLineId { get; set; }
        public string WorkstationLineName { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageCode { get; set; }
        public decimal QtyPerPackage { get; set; }
        public decimal QtyPerPallet { get; set; }
        public int MaxPackages { get; set; }
        public int PackageW { get; set; }
        public int PackageH { get; set; }
        public int PackageD { get; set; }
        public bool? PackageReturnable { get; set; }
        public string DEF { get; set; }
        public string PREFIX { get; set; }
        public string BC { get; set; }
        public string PackingCardUrl { get; set; }
    }
}
