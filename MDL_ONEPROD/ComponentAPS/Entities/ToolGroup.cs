using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_ToolGroup", Schema = "ONEPROD")]
    public class ToolGroup : IModelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public virtual Tool ParentTool { get; set; }
        //public int ParentToolId { get; set; }
        //public virtual Tool Tool { get; set; }
        //public int ToolId { get; set; }
        //public bool InUse { get; set; }

        //[NotMapped]
        //public bool locked { get; set; }
        //[NotMapped]
        //public bool Modified { get; set; }
    }
}
