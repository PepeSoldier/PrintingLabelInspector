using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponetMes.Entities
{
    [Table("MES_ProductionLogRawMaterial", Schema = "ONEPROD")]
    public class ProductionLogRawMaterial: IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual ProductionLog ProductionLog { get; set; }
        public int ProductionLogId { get; set; }

        public decimal UsedQty { get; set; }

        // Kod  surowca
        [MaxLength(50)]
        public string PartCode { get; set; }

        // Wygenerowany numer na Barkodzie
        public string BatchSerialNo { get; set; }

        public bool Deleted { get; set; }
    }
}