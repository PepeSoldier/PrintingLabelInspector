using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_Warehouse", Schema = "iLOGIS")]
    public class Warehouse : IModelEntity, IModelDeletableEntity
    {
        //[Key]
        public int Id { get; set; }
        //[MaxLength(25)]
        public string Code { get; set; }
	    public string Name { get; set; }
        public bool Deleted { get; set; }
        //TODO: warehouse może mieć subwarehouse więc trzeba dodać ParentWarehouse (podobnie jak w wareouselocation)
        public int QtyOfSubLocations { get; set; }

        //[ForeignKey("ParentWarehouseId")]
        public virtual Warehouse ParentWarehouse { get; set; }
        public int? ParentWarehouseId { get; set; }

        //[MaxLength(50)]
        //public string Description { get; set; }
        //[ForeignKey("AccountingWarehouseId")]
        public virtual Warehouse AccountingWarehouse { get; set; }
        public int? AccountingWarehouseId { get; set; }

        public WarehouseTypeEnum WarehouseType { get; set; }
        
        public bool IndependentSerialNumber { get; set; }

        public bool isMRP { get; set; }

        /// <summary>
        /// Zła nazwa! ma być isOutOfScope
        /// </summary>
        public bool isOutOfScore { get; set; }

        public bool isProduction { get; set; }

        [NotMapped]
        public double CurrentUsage { get; set; }
        [NotMapped]
        public double BoxAvgQty { get; set; }
        [NotMapped]
        public int BoxesCapacitySum { get; set; }
        [NotMapped]
        public int BoxesCount { get; set; }
        [NotMapped]
        public string Color { get; set; }
        //[NotMapped]
        //public Warehouse GetAccountingWarehouse
        //{
        //    get { return AccountingWarehouse == null ? this : AccountingWarehouse; }
        //}

        public string LabelLayoutFileName { get; set; }
    }
}
