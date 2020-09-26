using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_ToolMachine", Schema = "ONEPROD")]
    public class ToolMachine : IModelEntity
    {
        public int Id { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int MachineId { get; set; }

        public virtual Tool Tool { get; set; }
        public int ToolId { get; set; }

        public bool Placed { get; set; }
        public bool InUse { get; set; }
        public bool Preffered { get; set; }
    }
}
