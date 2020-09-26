using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_AutomaticRules", Schema = "iLOGIS")]
    public class AutomaticRule : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string PREFIX { get; set; }
        [MaxLength(100)]
        public string WorkstationName { get; set; }
        [MaxLength(255)]
        public string LineNames { get; set; }

        [Required, Range(0, Int32.MaxValue)]
        public int MaxPackages { get; set; }

        [Required, Range(0, Int32.MaxValue)]
        public int SafetyStock { get; set; }

        [Required, Range(0, Int32.MaxValue)]
        public int MaxBomQty { get; set; }

        public bool CheckOnly { get; set; }

        //package rules
        public int? PackageId { get; set; }
        public string PackageName { get; set; }
        public int QtyPerPackage { get; set; }
        public int PackagesPerPallet { get; set; }
        public int PalletW { get; set; }
        public int PalletD { get; set; }
        public int PalletH { get; set; }
        public decimal WeightGross { get; set; }

        public bool IsPackageType { get; set; }

        public bool Active { get; set; }

        public DateTime LastChange { get; set; }
        public string UserName { get; set; }
    }
}
