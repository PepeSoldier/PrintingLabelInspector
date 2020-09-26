using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_Delivery", Schema = "iLOGIS")]
    public class Delivery : IModelEntity
    {
        public int Id { get; set; }
        
        public virtual User User { get; set; }
        public string UserId { get; set; }

        public virtual Contractor Supplier { get; set; }
        public int SupplierId { get; set; }

        [MaxLength(25)]
        public string DocumentNumber { get; set; }

        public DateTime DocumentDate { get; set; }
        public DateTime StampTime { get; set; }

        public EnumDeliveryStatus EnumDeliveryStatus { get; set; }

        public bool Deleted { get; set; }

        public ICollection<DeliveryItem> DeliveryItems { get; set; }

        public string Guid { get; set; }

        public string ExternalId { get; set; }
        public string ExternalUserName { get; set; }
    }
}