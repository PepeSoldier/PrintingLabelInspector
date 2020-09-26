using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Interfaces;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_Tool", Schema = "ONEPROD")]
    public class Tool : IModelEntity
    {
        public Tool()
        {
            NM_Connected = (ToolGroupId != null);
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        
        //public int ParentToolId { get; set; }

        public virtual ToolGroup ToolGroup { get; set; }
        public int? ToolGroupId { get; set; }

        [NotMapped]
        public int NM_ItemGroupsCount { get; set; }

        [NotMapped]
        public bool NM_Connected { get; set; }

        [NotMapped]
        public bool Locked { get; set; }
        [NotMapped]
        public bool Modified { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
