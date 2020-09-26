using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_WarehouseItem", Schema = "iLOGIS")]
    public class WarehouseItem : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public int WarehouseId { get; set; }

        public virtual ItemWMS ItemGroup { get; set; }
        public int ItemGroupId { get; set; }
        
        public int QtyPerLocation { get; set; }

        [NotMapped]
        public decimal UtylizationPerOnepiece {
            get
            {
                if(QtyPerLocation == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Ceiling(((decimal)1 / (decimal)QtyPerLocation) * 10000) / 10000;
                }
            }
        }

        public int HoursCoverage { get; set; }
        public bool Deleted { get; set; }

        //[NotMapped]
        //public int MaxStore { get { if (Warehouse != null) { return Warehouse.QtyOfSubLocations * QtyPerLocation; } else { return 0; } } }
        //[NotMapped]
        //public virtual Buffor_Box Box { get; set; }
        [NotMapped]
        public int Qty { get; set; }
    }
}
