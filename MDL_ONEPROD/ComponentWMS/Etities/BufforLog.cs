using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("PREPROD_BufforLog", Schema = "ONEPROD")]
    public class BufforLog : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(9)]
        public string ANC { get; set; }

        public virtual ItemOP ItemGroup { get; set; }
        public int ItemGroupId { get; set; }

        public virtual Warehouse Box { get; set; }
        public int BoxId { get; set; }

        public int Qty { get; set; }
        public int UsedBoxes { get; set; }
        public int TotalUsedBoxes { get; set; }
        public int TotalBoxes { get; set; }
        public int MaxStore { get; set; }
        public DateTime Time { get; set; }
    }
}
