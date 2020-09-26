using MDL_BASE.Enums;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentBase.Entities
{
    [Table("CORE_BOM", Schema = "ONEPROD")]
    public class BomOneprod
    {
        [Key]
        public int Id { get; set; }

        public virtual ItemOP ChildItem { get; set; }
        public int? ChildItemId { get; set; }

        public virtual ItemOP ParentItem { get; set; }
        public int? ParentItemId { get; set; }

        public int Level { get; set; }
        public decimal Qty { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}