using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PRD.ComponentSchedule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PRD.Model
{
    [Table("ProdOrder_Status", Schema = "PRD")]
    public class ProdOrderStatus : IModelEntity
    {
        public int Id { get; set; }

        public virtual ProductionOrder Order { get; set; }
        public int OrderId { get; set; }

        [MaxLength(8)]
        public string StatusName { get; set; }

        public EnumOrderState StatusState { get; set; }
        [MaxLength(8)]
        public string StatusInfo { get; set; }
        [MaxLength(100)]
        public string StatusInfoExtra { get; set; }
        [MaxLength(100)]
        public string StatusInfoExtra2 { get; set; }

        public int StausInfoExtraNumber { get; set; }
    }
}