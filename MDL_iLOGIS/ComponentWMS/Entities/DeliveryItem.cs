using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_DeliveryItem", Schema = "iLOGIS")]
    public class DeliveryItem : IModelEntity
    {
        public int Id { get; set; }
        
        public virtual User User { get; set; }
        public string UserId { get; set; }
        
        public virtual Delivery Delivery { get; set; }
        public int DeliveryId { get; set; }
        
        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual PackageItem PackageItem { get; set; }
        public int? PackageItemId { get; set; }

        public int NumberOfPackages { get; set; }
        public decimal QtyInPackage { get; set; }
        public decimal TotalQty { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public StatusEnum StockStatus { get; set; }
        public EnumMovementType MovementType { get; set; }
        public string DestinationWarehouseCode { get; set; }
        public bool IsSpecialStock { get; set; }

        public bool AdminEntry { get; set; }
        public bool OperatorEntry { get; set; }
        public bool WasPrinted { get; set; }

        public bool IsLocationAssigned { get; set; }
        public bool IsLocated { get; set; }
        public decimal TotalLocatedQty { get; set; }

        public bool Deleted { get; set; }
    }
    
}
