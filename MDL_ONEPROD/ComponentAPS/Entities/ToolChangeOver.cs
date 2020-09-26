using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_ToolChangeOver", Schema = "ONEPROD")]
    public class ToolChangeOver : IModelEntity
    {
        public int Id { get; set; }

        public virtual Tool Tool1 {get;set;}
        public int Tool1Id { get; set; }

        public virtual Tool Tool2 {get;set;}
        public int? Tool2Id { get; set; }

        public int Time { get; set; } //minutes
    }
}