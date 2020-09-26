using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_PackageItem", Schema = "iLOGIS")]
    public class PackageItem : IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual Package Package { get; set; }
        public int PackageId { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public int? WarehouseId { get; set; }

        public virtual WarehouseLocationType WarehouseLocationType { get; set; }
        public int WarehouseLocationTypeId { get; set; }

        public PickingStrategyEnum PickingStrategy { get; set; }

        public decimal QtyPerPackage { get; set; }
        public int PackagesPerPallet { get; set; }
        public int PalletW { get; set; }
        public int PalletD { get; set; }
        public int PalletH { get; set; }
        public decimal WeightGross { get; set; }


        [NotMapped]
        public decimal QtyPerPallet { get { return QtyPerPackage * PackagesPerPallet; } }
        [NotMapped]
        public decimal WeightNet { get { return CalculateWeight(); } }

        public bool Deleted { get; set; }

        private decimal CalculateWeight()
        {
            decimal w = 0;
            if (ItemWMS != null && Package != null)
            {
                w = Package.Weight + ItemWMS.Weight * QtyPerPackage;
            }
            return w;
        }
    }
}
