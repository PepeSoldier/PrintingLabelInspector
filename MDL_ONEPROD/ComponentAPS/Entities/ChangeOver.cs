using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_ChangeOver", Schema = "ONEPROD")]
    public class ChangeOver : IModelEntity
    {
        public int Id { get; set; }
        public int ToolModification { get; set; }
        public int MachineToolChange { get; set; }
        public int ToolChange { get; set; }
        public int CatergoyChange { get; set; }
        public int AncChange { get; set; }
        public int AreaID { get; set; }
    }
}
