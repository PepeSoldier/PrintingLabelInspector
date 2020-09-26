using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_PickingList", Schema = "iLOGIS")]
    public class PickingList : IModelEntity
    {
        public PickingList()
        {
            GuidCreationDate = new DateTime(1900, 1, 1);
        }
        public int Id { get; set; }

        public EnumPickingListStatus Status { get; set; }
        //public EnumDeliveryListStatus StatusLF { get; set; }

        public virtual ProductionOrder WorkOrder { get; set; }
        public int WorkOrderId { get; set; }

        public virtual Transporter Transporter { get; set; }
        public int TransporterId { get; set; }

        public string Guid { get; set; }
        public DateTime GuidCreationDate { get; set; }
    }
}
