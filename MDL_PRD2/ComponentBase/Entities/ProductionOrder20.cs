using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PRD.Model //MDL_PFEP.Model.PFEP
{
    [Table("ProdOrder_20", Schema = "PRD")]
    public class Prodorder20 : IModelEntity
    {
        [NotMapped]
        public static string TableName = "PRD.ProdOrder_20";

        public int Id { get; set; }

        public virtual ProductionOrder Order { get; set; }
        public int OrderId { get; set; }

        public int PartQty { get; set; }
        public int PartQtyRemain { get; set; }
        public int PartNumber { get; set; }

        public DateTime PartStartDate { get; set; }
    }
}