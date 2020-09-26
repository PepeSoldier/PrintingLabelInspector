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
    [Table("WMS_TransporterLog", Schema = "iLOGIS")]
    public class TransporterLog : IModelEntity
    {
        public int Id { get; set; }

        public virtual Transporter Transporter { get; set; }
        public int TransporterId { get; set; }

        /// <summary>Item that should be picked or delivered to line</summary>
        public virtual ItemWMS ItemWMS { get; set; }
        public int? ItemWMSId { get; set; }

        /// <summary>Qty picked/delivered/missing/problem/other</summary>
        public decimal ItemQty { get; set; }

        /// <summary>Workorder numer.No foreign key because workorders may be deleted from database.</summary>
        public string WorkorderNumber { get; set; }

        /// <summary>PNC of work order</summary>
        public string ProductItemCode { get; set; }

        //Numer picking listy, delivery listy, numer dostawy
        //Stanowisko,lokacja

        public EnumTransporterLogEntryType EntryType { get; set; }
        public int RelatedObjectId { get; set; }

        public EnumTransportingStatus Status { get; set; }

        public string Comment { get; set; }
        public string Location { get; set; }
        
        public DateTime? TimeStamp { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        public DateTime DateFrom { get; set; }
        [NotMapped]
        public DateTime DateTo { get; set; }
    }
}
