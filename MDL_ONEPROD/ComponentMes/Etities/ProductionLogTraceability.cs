using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.Etities
{
    [Table("MES_ProductionLogTraceability", Schema = "ONEPROD")]
    public class ProductionLogTraceability : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual ProductionLog Parent { get; set; }
        public int ParentId { get; set; }

        public virtual ProductionLog Child { get; set; }
        public int? ChildId { get; set; }

        [MaxLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(25)]
        public string SerialNumber { get; set; }

        public DateTime? ProductionDate { get; set; }

        public bool Deleted { get; set; }

    }
}