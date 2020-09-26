using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_Movement", Schema = "iLOGIS")]
    public class Movement : IModelEntity
    {
        public int Id { get; set; }
        
        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual WarehouseLocation SourceLocation { get; set; }
        public int SourceLocationId { get; set; }
        
        public virtual Warehouse SourceWarehouse { get; set; }
        public int SourceWarehouseId{ get; set; }

        public string SourceStockUnitSerialNumber { get; set; }

        public virtual WarehouseLocation DestinationLocation { get; set; }
        public int DestinationLocationId { get; set; }
        
        public virtual Warehouse DestinationWarehouse { get; set; }
        public int DestinationWarehouseId { get; set; }

        public string DestinationStockUnitSerialNumber { get; set; }

        public decimal QtyMoved { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [MaxLength(200)]
        public string FreeText1 { get; set; }
        [MaxLength(200)]
        public string FreeText2 { get; set; }

        public EnumMovementType Type { get; set; }

        public DateTime Date { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

        [MaxLength(150)]
        public string ExternalId { get; set; }
        [MaxLength(100)]
        public string ExternalUserName { get; set; }

        public DateTime? ExportDateTime { get; set; }
    }
}
