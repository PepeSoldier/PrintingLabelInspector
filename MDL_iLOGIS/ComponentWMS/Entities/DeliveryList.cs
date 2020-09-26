using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using MDL_iLOGIS.ComponentWMS.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_DeliveryList", Schema = "iLOGIS")]
    public class DeliveryList: IModelEntity
    {
        public int Id { get; set; }

        public virtual ProductionOrder WorkOrder { get; set; }
        public int WorkOrderId { get; set; }

        public virtual Transporter Transporter { get; set; }
        public int TransporterId { get; set; }

        public EnumDeliveryListStatus Status { get; set; }
    }
}
