using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.OEEProd
{
    [Table("OEE_MachineReason", Schema = "ONEPROD")]
    public class MachineReason : IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int MachineId { get; set; }

        public virtual Reason Reason { get; set; }
        public int ReasonId { get; set; }

        //public virtual MachineReason ParentReason { get; set; }
        //public int ParentReasonId { get; set; }

        public bool Deleted { get; set; }
    }
}