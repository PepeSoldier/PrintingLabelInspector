using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("CORE_ClientOrder", Schema = "ONEPROD")]
    public class ClientOrder : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }
        public string Resource { get; set; }

        public Contractor Client { get; set; }
        public int? ClientId { get; set; }

        [MaxLength(50)]
        public string ClientItemCode { get; set; }
        [MaxLength(250)]
        public string ClientItemName { get; set; }

        [MaxLength(50)]
        public string ItemCode { get; set; }
        [MaxLength(250)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        public string OrderNo { get; set; }

        public int Qty_Total { get; set; }
        public int Qty_Produced { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        //public int Qty_Remain { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public bool Deleted { get; set; }

        public int RefOrderId { get; set; }

        [NotMapped]
        public int MarginRight { get; set; }
        [NotMapped]
        public int MarginLeft { get; set; }
        [NotMapped]
        public int Width { get; set; }
        [NotMapped]
        public string BackgroundColor { get; set; }
    }
}
