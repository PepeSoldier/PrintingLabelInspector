using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_ToolItemGroup", Schema = "ONEPROD")]
    public class ItemGroupTool : IModelEntity
    {
        public int Id { get; set; }

        public virtual ItemOP ItemGroup { get; set; }
        public int ItemGroupId { get; set; }

        public virtual Tool Tool { get; set; }
        public int ToolId { get; set; }
    }
}
