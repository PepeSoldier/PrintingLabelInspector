using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.OEEProd
{
    [Table("OEE_MachineTarget", Schema = "ONEPROD")]
    public class MachineTarget : IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual ResourceOP Resource { get; set; }
        public int ResourceId { get; set; }

        public int ReasonTypeId { get; set; }

        public decimal Target { get; set; }
        
        public bool Deleted { get; set; }
    }
}